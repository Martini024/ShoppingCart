using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCart.Models
{
    public class Activation
    {
        [Required]
        [Key, Column(Order = 0)]
        public string ProductId { get; set; }

        [Required]
        [Key, Column(Order = 1)]
        public string Code { get; set; }

        public string OrderId { get; set; }

    }
}