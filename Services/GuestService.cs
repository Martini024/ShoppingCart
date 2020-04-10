using System;
using ShoppingCart.Models;
using ShoppingCart.Database;

namespace ShoppingCart.Services
{
    public class GuestService
    {
        public string guestId { get; }
        public GuestService()
        {
            guestId = Guid.NewGuid().ToString();
        }
    }
}