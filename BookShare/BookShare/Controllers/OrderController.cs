using AutoMapper;
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

            BookSharingEntities db = new BookSharingEntities();


            Dictionary<int, List<int>> dict = new Dictionary<int, List<int>>();
            
            //Book er id seller onujayi alada kora
            foreach (int id in bookIds)
            {
                BookList b = new BookList();
                var bookDetails = b.bookDetails(id);
                

                if (dict.ContainsKey(bookDetails.SellerId))
                {
                    dict[bookDetails.SellerId].Add(id);
                }
                else
                {
                    dict.Add(bookDetails.SellerId, new List<int>());
                    dict[bookDetails.SellerId].Add(id);
                }

            }

            //per seller er booklist er id diye kaj kora
            foreach (var seller in dict)
            {
                //per seller er book list neya
                List<int> sBookIds = seller.Value;

                Order order = new Order();
                order.UserId = System.Convert.ToInt32(Session["userId"].ToString());
                order.Status = "pending";
                db.Orders.Add(order);
                db.SaveChanges();

                foreach (int id in sBookIds)
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
            }
            Session.Remove("cart");

            return RedirectToAction("UserDash", "Users");
        }
        public ActionResult OrderHistoryOfUser()
        {
            int id = System.Convert.ToInt32(Session["userId"].ToString());
            BookSharingEntities db = new BookSharingEntities();
            var orders = (from x in db.Orders
                          where x.UserId.Equals(id)
                          select x).ToList();

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Book,BookModel>());
            var mapper = new Mapper(config);

            var config1 = new MapperConfiguration(cfg => cfg.CreateMap<Shop, ShopModel>());
            var mapper1 = new Mapper(config1);

            var config2 = new MapperConfiguration(cfg => cfg.CreateMap<User, UserModel>());
            var mapper2 = new Mapper(config2);

            List<OrderOrderDetailsModel> oods = new List<OrderOrderDetailsModel>();
            foreach (var order in orders)
            {
                List<OrderDetailsModel> odrDetails = new List<OrderDetailsModel>();
                foreach(var x in order.OrderDetails)
                {
                    OrderDetailsModel odm = new OrderDetailsModel();
                    odm.Id = x.Id;
                    odm.OrderId = x.OrderId;
                    odm.BookId = x.BookId;
                    odm.SellerId = x.SellerId;
                    odm.Quantity = x.Quantity;
                    odm.price = x.price;
                    odm.ShopId = x.ShopId;
                    odm.Book = mapper.Map<BookModel>(x.Book);
                    odm.Shop = mapper1.Map<ShopModel>(x.Shop);
                    odm.User = mapper2.Map<UserModel>(x.User);
                    odrDetails.Add(odm);
                }
                OrderOrderDetailsModel ood = new OrderOrderDetailsModel();
                ood.Id = order.Id;
                ood.UserId = order.UserId;
                ood.Status = order.Status;
                ood.orderDetails = odrDetails;

                oods.Add(ood);
            }
            

            return View(oods);
        }

        public ActionResult CancleOrder(int id)
        {
            BookSharingEntities db = new BookSharingEntities();
            var order = (from del in db.Orders
                          where del.Id.Equals(id)
                          select del).FirstOrDefault();
            //db.Orders.Remove(order);
            Order can = new Order();
            can.Id = order.Id;
            can.UserId = order.UserId;
            can.Status = "Cancled";

            db.Entry(order).CurrentValues.SetValues(can);

            db.SaveChanges();
            return RedirectToAction("OrderHistoryOfUser");
        }

        public ActionResult StockOutForSeller(int id)
        {
            BookSharingEntities db = new BookSharingEntities();
            var order = (from del in db.Orders
                         where del.Id.Equals(id)
                         select del).FirstOrDefault();
            
            Order can = new Order();
            can.Id = order.Id;
            can.UserId = order.UserId;
            can.Status = "Stock Out";

            db.Entry(order).CurrentValues.SetValues(can);

            db.SaveChanges();
            return RedirectToAction("OrderViewForSeller");
        }


    }
}