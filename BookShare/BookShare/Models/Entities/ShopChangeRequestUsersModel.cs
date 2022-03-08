using BookShare.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookShare.Models.Entities
{
    public class ShopChangeRequestUsersModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string ShopNumber { get; set; }
        public string ShopDocuments { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }

        public User User { get; set; }
    }
}