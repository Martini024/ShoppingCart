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
using ShoppingCart.Models;
using ShoppingCart.Database;
namespace ShoppingCart.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private ShoppingCartContext _dbContext;
        public ProductController(ILogger<ProductController> logger, ShoppingCartContext dbContext)
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

        public void SetNavBar()
        {
            string user = GetCurrentUser();
            if (HttpContext.User.Claims.FirstOrDefault() == null)
            {
                ViewData["Title"] = "ShoppingCart";
                ViewData["isLoggedIn"] = false;
            }
            else
            {
                ViewData["Title"] = "Hello, " + user;
                ViewData["isLoggedIn"] = true;
            }
            Cart cart = _dbContext.Carts.Where(c => c.UserId == user).FirstOrDefault();
            if (cart == null)
                ViewData["TotalQty"] = 0;
            else
                ViewData["TotalQty"] = cart.CartDetails.Sum(cd => cd.Qty);
        }
        public IActionResult Index()
        {
            SetNavBar();
            List<Product> products = _dbContext.Products.ToList();
            return View(products);
        }

        [HttpPost]
        public IActionResult Index(string searchBy)
        {
            if (searchBy == null)
                return RedirectToAction("Index", "Product");
            else
            {
                SetNavBar();
                ViewData["SearchBy"] = searchBy;
                List<Product> products = _dbContext.Products.Where(product => product.Name.ToLower().Contains(searchBy.ToLower()) || product.Description.ToLower().Contains(searchBy.ToLower())).ToList();
                return View(products);
            }
        }
    }
}
