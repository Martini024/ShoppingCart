using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Models;

namespace ShoppingCart.Database
{
    public class AccountRegister
    {
        public AccountRegister(ShoppingCartContext dbcontext)
        {
            // Generate hard-coded 4 users sharing 123 as password
            User user1 = new User();
            user1.UserId = "John";
            user1.Password = "123";
            dbcontext.Add(user1);
            user1.UserId = "Mary";
            dbcontext.Add(user1);
            user1.UserId = "Chris";
            dbcontext.Add(user1);
            user1.UserId = "Martini";
            dbcontext.Add(user1);

            dbcontext.SaveChanges();
        }
    }
}
