using BookShare.Auth;
using BookShare.Models.Database;
using BookShare.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace BookShare.Controllers
{
    [UserValidation]
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Order()
        {
            var x = Session["cart"].ToString();
            List<int> bookIds = new JavaScriptSerializer().Deserialize<List<int>>(x);

            Order order = new Order();
            order.UserId = System.Convert.ToInt32(Session["userId"].ToString());
            order.Status = "pending";

            BookSharingEntities db = new BookSharingEntities();
            db.Orders.Add(order);
            db.SaveChanges();

            foreach (int id in bookIds)
            {
                BookList b = new BookList();
                var bookDetails = b.bookDetails(id);
                bookDetails.BookQuantity = "1";

                OrderDetail od = new OrderDetail();
                od.OrderId = order.Id;
                od.BookId = bookDetails.BookId;
                od.SellerId = bookDetails.SellerId;
                od.Quantity = System.Convert.ToInt32(bookDetails.BookQuantity.ToString());
                od.price = bookDetails.Book.Price;
                od.ShopId = bookDetails.ShopId;

                db.OrderDetails.Add(od);
                db.SaveChanges();
            }

            Session.Remove("cart");

            return RedirectToAction("UserDash", "Users");
        }

        public ActionResult OrderHistory()
        {
            return View();
        }
    }
}