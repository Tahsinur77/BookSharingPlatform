using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookShare.Models.Entities
{
    public class OrderDetailsModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int BookId { get; set; }
        public int SellerId { get; set; }
        public int Quantity { get; set; }
        public string price { get; set; }
        public int ShopId { get; set; }

        public  BookModel Book { get; set; }
        public  ShopModel Shop { get; set; }
        public  UserModel User { get; set; }
    }
}