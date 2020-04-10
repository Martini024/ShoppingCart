using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Models;
using ShoppingCart.Services;

namespace ShoppingCart.Database
{
    public class MockData
    {
        public MockData(ShoppingCartContext dbcontext, GuestService guest)
        {
            // Generate hard-coded 4 users sharing 123 as password
            User user = new User();
            user.UserId = "John";
            user.Password = "123";
            dbcontext.Add(user);
            dbcontext.SaveChanges();

            user.UserId = "Mary";
            dbcontext.Add(user);
            dbcontext.SaveChanges();

            user.UserId = "Chris";
            dbcontext.Add(user);
            dbcontext.SaveChanges();

            user.UserId = "Martini";
            dbcontext.Add(user);
            dbcontext.SaveChanges();

            user.UserId = guest.guestId;
            user.Password = guest.guestId;
            dbcontext.Add(user);
            dbcontext.SaveChanges();

            // Generate hard-coded 6 products
            Product product = new Product();
            product.ProductId = Guid.NewGuid().ToString();
            product.Name = ".Net Charts";
            product.Description = "Brings a powerful charting capabilities to your .Net applications.";
            product.Price = 99;
            product.Image = "1.jpg";
            dbcontext.Add(product);
            dbcontext.SaveChanges();

            product.ProductId = Guid.NewGuid().ToString();
            product.Name = ".Net Paypal";
            product.Description = "Integrate your .Net apps with Paypal the easy way!";
            product.Price = 69;
            product.Image = "2.jpg";
            dbcontext.Add(product);
            dbcontext.SaveChanges();

            product.ProductId = Guid.NewGuid().ToString();
            product.Name = ".Net ML";
            product.Description = "Supercharged .Net machine learning libraries.";
            product.Price = 299;
            product.Image = "3.jpg";
            dbcontext.Add(product);
            dbcontext.SaveChanges();

            product.ProductId = Guid.NewGuid().ToString();
            product.Name = ".Net Analytics";
            product.Description = "Perform data mining and analytics easily in .Net.";
            product.Price = 299;
            product.Image = "4.jpg";
            dbcontext.Add(product);
            dbcontext.SaveChanges();

            product.ProductId = Guid.NewGuid().ToString();
            product.Name = ".Net Logger";
            product.Description = "Logs and aggregates events easily in your .Net apps.";
            product.Price = 49;
            product.Image = "5.jpg";
            dbcontext.Add(product);
            dbcontext.SaveChanges();

            product.ProductId = Guid.NewGuid().ToString();
            product.Name = ".Net Numerics";
            product.Description = "Powerful numerical methods for your .Net simulations.";
            product.Price = 199;
            product.Image = "6.jpg";
            dbcontext.Add(product);
            dbcontext.SaveChanges();
        }
    }
}
