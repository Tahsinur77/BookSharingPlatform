using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookShare.Models.Entities
{
    public class BookDetailModel
    {
        public int Id { get; set; }
        public int SellerId { get; set; }
        public int BookId { get; set; }
        public int ShopId { get; set; }
        [Required]
        public string BookQuantity { get; set; }
    }
}