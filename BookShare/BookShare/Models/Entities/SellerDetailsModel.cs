using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookShare.Models.Entities
{
    public class SellerDetailsModel
    {
        public int Id { get; set; }
        public int SellerId { get; set; }
        public string Nid { get; set; }
        public string Status { get; set; }
        public string ShopNumber { get; set; }
        public string ShopDocuments { get; set; }
    }
}