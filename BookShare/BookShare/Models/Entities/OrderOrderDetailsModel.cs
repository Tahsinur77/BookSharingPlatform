using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookShare.Models.Entities
{
    public class OrderOrderDetailsModel:OrderModel
    {
        public List<OrderDetailsModel> orderDetails { set; get; }

        public OrderOrderDetailsModel()
        {
            orderDetails = new List<OrderDetailsModel>();
        }
    }
}