using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index()
        {
            var user = HttpContext.User.Claims.FirstOrDefault();
            ViewData["Title"] = user == null ? "ShoppingCart" : "Hello, " + user.Value;
            ViewData["isLoggedIn"] = user == null ? false : true;
            // FIXME: repetatvie code
            if (user != null)
            {
                Cart cart = _dbContext.Carts.Where(c => c.UserId == user.Value).FirstOrDefault();
                if (cart == null)
                    ViewData["TotalQty"] = 0;
                else
                    ViewData["TotalQty"] = cart.CartDetails.Where(cd => cd.CartId == cart.CartId).Count();
                _logger.LogInformation(ViewData["TotalQty"].ToString());
            }
            List<Product> products = _dbContext.Products.ToList();
            return View(products);
        }

        [HttpPost]
        public IActionResult Index(string searchBy)
        {
            // FIXME: repetatvie code
            if (searchBy == null)
                return RedirectToAction("Index", "Product");
            var user = HttpContext.User.Claims.FirstOrDefault();
            ViewData["Title"] = user == null ? "ShoppingCart" : "Hello, " + user.Value;
            ViewData["isLoggedIn"] = user == null ? false : true;
            if (user != null)
            {
                Cart cart = _dbContext.Carts.Where(c => c.UserId == user.Value).FirstOrDefault();
                if (cart == null)
                    ViewData["TotalQty"] = 0;
                else
                    ViewData["TotalQty"] = cart.CartDetails.Where(cd => cd.CartId == cart.CartId).Count();
            }
            if (searchBy == null)
            {
                return RedirectToAction("Index", "Purchase");
            }
            else
            {
                List<Product> products = _dbContext.Products.Where(product => product.Name.ToLower().Contains(searchBy.ToLower()) || product.Description.ToLower().Contains(searchBy.ToLower())).ToList();
                return View(products);
            }
        }
    }
}
