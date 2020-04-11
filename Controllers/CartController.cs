using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using ShoppingCart.Models;
using ShoppingCart.Database;

namespace ShoppingCart.Controllers
{
    public class CartController : Controller
    {
        private readonly ILogger<CartController> _logger;
        private ShoppingCartContext _dbContext;
        public CartController(ILogger<CartController> logger, ShoppingCartContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        public string GetCurrentUser()
        {
            var user = HttpContext.User.Claims.FirstOrDefault();
            if (user == null)
            {
                if (HttpContext.Session.GetString("GuestId") != null)
                {
                    return HttpContext.Session.GetString("GuestId");
                }
                else
                {
                    string guestId = Guid.NewGuid().ToString();
                    HttpContext.Session.SetString("GuestId", guestId);
                    _dbContext.Users.Add(new User()
                    {
                        UserId = guestId,
                        Password = Guid.NewGuid().ToString()
                    });
                    _dbContext.SaveChanges();
                    return guestId;
                }
            }
            else
                return user.Value;
        }
        public IActionResult Index()
        {
            string userId = GetCurrentUser();
            Cart cart = _dbContext.Carts.Where(c => c.UserId == userId).FirstOrDefault();
            return View(cart);
        }

        [HttpPost]
        public IActionResult AddToCart(string productId, double price)
        {
            if (_dbContext.Products.Where(p => p.ProductId == productId && p.Price == price).Any())
            {
                string userId = GetCurrentUser();
                var currentCart = _dbContext.Carts.Where(c => c.UserId == userId);
                Cart cart = new Cart();
                if (!currentCart.Any())
                {
                    cart.UserId = userId;
                    CreateCart(productId, price, cart);
                    _dbContext.Carts.Add(cart);
                }
                else
                {
                    cart = currentCart.First();
                    UpdateCurrentCart(productId, price, cart);
                    _dbContext.Carts.Update(cart);
                }
                _dbContext.SaveChanges();
                return Json(cart.TotalQty);
            }
            else
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public void CreateCart(string productId, double price, Cart cart)
        {
            cart.CartId = Guid.NewGuid().ToString();
            cart.Total = price;
            cart.CartDetails = new List<CartDetail>() {
                    new CartDetail
                    {
                        CartId = cart.CartId,
                        ProductId = productId,
                        Qty = 1
                    }
                };
            cart.TotalQty = cart.CartDetails.Sum(cd => cd.Qty);
        }
        public void UpdateCurrentCart(string productId, double price, Cart cart)
        {
            cart.Total += price;
            var currentCartDetail = _dbContext.CartDetails.Where(cd => cd.CartId == cart.CartId && cd.ProductId == productId);
            if (!currentCartDetail.Any())
            {
                cart.CartDetails.Add(new CartDetail
                {
                    CartId = cart.CartId,
                    ProductId = productId,
                    Qty = 1
                });
            }
            else
            {
                currentCartDetail.First().Qty++;
            }
            cart.TotalQty = cart.CartDetails.Sum(cd => cd.Qty);
        }

        [HttpPost]
        public void UpdateQty(string cartId, string productId, int changeTo)
        {
            if (changeTo >= 0)
            {
                var cartDetail = _dbContext.CartDetails.Where(cd => cd.CartId == cartId && cd.ProductId == productId).FirstOrDefault();
                var cart = _dbContext.Carts.Where(c => c.CartId == cartId).FirstOrDefault();
                if (cart != null && cartDetail != null)
                {
                    cart.Total -= cartDetail.Product.Price * (cartDetail.Qty - changeTo);
                    if (changeTo > 0)
                        cartDetail.Qty = changeTo;
                    else
                        _dbContext.CartDetails.Remove(cartDetail);
                }
                _dbContext.SaveChanges();
            }
            return;
        }

        [Authorize]
        public IActionResult Checkout()
        {
            double total = MergeGuestCart();
            return RedirectToAction("Index", "Payment", new { total = total });
        }

        public IActionResult PlaceOrder()
        {
            CreateOrder();
            return RedirectToAction("Index", "Purchase");
        }

        public double MergeGuestCart()
        {
            var userId = HttpContext.User.Claims.First().Value;
            var guestId = HttpContext.Session.GetString("GuestId");
            var userCart = _dbContext.Carts.Where(c => c.UserId == userId).FirstOrDefault();
            var guestCart = _dbContext.Carts.Where(c => c.UserId == guestId).FirstOrDefault();
            var guest = _dbContext.Users.Where(u => u.UserId == guestId).FirstOrDefault();
            double total = 0;
            if (guestId != null)
            {
                if (guestCart != null)
                {
                    if (userCart != null)
                    {
                        foreach (var guestCartDetail in guestCart.CartDetails)
                        {
                            var userCartDetail = userCart.CartDetails.Where(cd => cd.ProductId == guestCartDetail.ProductId).FirstOrDefault();
                            if (userCartDetail != null)
                            {
                                userCartDetail.Qty += guestCartDetail.Qty;
                            }
                            else
                            {
                                userCart.CartDetails.Add(new CartDetail()
                                {
                                    CartId = userCart.CartId,
                                    ProductId = guestCartDetail.ProductId,
                                    Qty = guestCartDetail.Qty
                                });
                            }
                        }
                        userCart.Total += guestCart.Total;
                        total = userCart.Total;
                    }
                    else
                    {
                        guestCart.UserId = userId;
                        total = guestCart.Total;
                    }
                }
                else
                {
                    if (userCart != null)
                        total = userCart.Total;
                    else
                        total = 0;
                }
                if (guest != null)
                    _dbContext.Users.Remove(guest);
                _dbContext.SaveChanges();
                HttpContext.Session.Remove("GuestId");
            }
            else
            {
                if (userCart != null)
                    total = userCart.Total;
                else
                    total = 0;
            }
            return total;
        }
        public void CreateOrder()
        {
            // 1. create order
            // _order.OrderId = Guid.NewGuid().ToString();
            // _order.UserId = GetCurrentUser();
            // _order.UtcDateTime = DateTime.Now.ToUniversalTime();

        }
        // TODO: Assign activation code
        // move item to order tbl
        // assign activation code
        // go to my purchase page
    }
}
