using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCart.Models
{
    public class CartDetail
    {
        [Required]
        [Key, Column(Order = 0)]
        public string UserId { get; set; }

        [Required]
        [Key, Column(Order = 1)]
        public string ProductId { get; set; }

        [Required]
        public int Qty { get; set; }

    }
}