//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BookShare.Models.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public User()
        {
            this.BookDetails = new HashSet<BookDetail>();
            this.OrderDetails = new HashSet<OrderDetail>();
            this.Orders = new HashSet<Order>();
            this.Returns = new HashSet<Return>();
            this.Returns1 = new HashSet<Return>();
            this.SellerDetails = new HashSet<SellerDetail>();
            this.ShopChangeRequests = new HashSet<ShopChangeRequest>();
            this.Shops = new HashSet<Shop>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Picture { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
    
        public virtual ICollection<BookDetail> BookDetails { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Return> Returns { get; set; }
        public virtual ICollection<Return> Returns1 { get; set; }
        public virtual ICollection<SellerDetail> SellerDetails { get; set; }
        public virtual ICollection<ShopChangeRequest> ShopChangeRequests { get; set; }
        public virtual ICollection<Shop> Shops { get; set; }
    }
}
