using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookShare.Models.Entities
{
    public class BookDetailBookShopUserModel
    {
        public int Id { get; set; }
        public int SellerId { get; set; }
        public int BookId { get; set; }
        public int ShopId { get; set; }
        public string BookQuantity { get; set; }

        public BookAuthorModel Book { get; set; }
        public ShopModel Shop { get; set; }
        public UserModel User { get; set; }
    }
}