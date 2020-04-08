using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCart.Models
{
    public class Cart
    {
        public string UserId { get; set; }

        [Required]
        public double Total { get; set; }
    }
}