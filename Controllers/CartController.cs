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
        private Cart _cart;
        public CartController(ILogger<CartController> logger, ShoppingCartContext dbContext, Cart cart)
        {
            _logger = logger;
            _dbContext = dbContext;
            _cart = cart;
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
                if (!currentCart.Any())
                {
                    _cart.UserId = userId;
                    CreateCart(productId, price, _cart);
                    _dbContext.Carts.Add(_cart);
                }
                else
                {
                    _cart = currentCart.First();
                    UpdateCurrentCart(productId, price, _cart);
                    _dbContext.Carts.Update(_cart);
                }
                _dbContext.SaveChanges();
                return Json(_cart.TotalQty);
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

        // TODO: Update qty method
        // - input qtyChanged orderdetail

        // TODO: Checkout
        // Check login first
        // Bring to payment controller
        [Authorize]
        public IActionResult Checkout()
        {
            RemoveFromCart();
            AddToOrder();
            return RedirectToAction("Index", "Purchase");
        }

        public void RemoveFromCart()
        {

        }
        public void AddToOrder()
        {

        }
        // TODO: Assign activation code
        // move item to order tbl
        // assign activation code
        // go to my purchase page
    }
}
