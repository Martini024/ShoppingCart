using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCart.Models
{
    public class OrderDetail
    {
        [Required]
        [Key, Column(Order = 0)]
        public string OrderId { get; set; }

        [Required]
        [Key, Column(Order = 0)]
        public string ProductId { get; set; }

        [Required]
        public int Qty { get; set; }

    }
}