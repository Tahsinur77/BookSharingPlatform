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
    
    public partial class Order
    {
        public Order()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
            this.Returns = new HashSet<Return>();
            this.Sells = new HashSet<Sell>();
        }
    
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Status { get; set; }
    
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Return> Returns { get; set; }
        public virtual ICollection<Sell> Sells { get; set; }
    }
}
