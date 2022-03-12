using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookShare.Models.Entities
{
    public class OrderModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Status { get; set; }
    }
}