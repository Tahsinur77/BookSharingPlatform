using AutoMapper;
using BookShare.Auth;
using BookShare.Models.Database;
using BookShare.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookShare.Controllers
{
    [UserValidation]
    public class SellersController : Controller
    {
        // GET: Sellers
        [HttpGet]
        public ActionResult SellerRegistration()
        {
            BookSharingEntities db = new BookSharingEntities();
            var sellerId = System.Convert.ToInt32(Session["userId"]);
            var flag = (from x in db.SellerDetails
                        where x.SellerId.Equals(sellerId)
                        select x).FirstOrDefault();

            if (flag != null && flag.Status == "painding")
            {
                TempData["error"] = "Already register for seller";
                return RedirectToAction("UserDash", "Users");
            }
            return View();
        }
        [HttpPost]
        public ActionResult SellerRegistration(SellerDetailsModel sellerDetailsModel, HttpPostedFileBase ShopDocuments)
        {
            if (ModelState.IsValid)
            {
                BookSharingEntities db = new BookSharingEntities();
                var check = (from sNum in db.Shops
                             where sNum.ShopNumber.Equals(sellerDetailsModel.ShopNumber)
                             select sNum).FirstOrDefault();

                if (check == null)
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<SellerDetailsModel, SellerDetail>());
                    var mapper = new Mapper(config);
                    var sellerDetails = mapper.Map<SellerDetail>(sellerDetailsModel);

                    if (ShopDocuments != null)
                    {

                        string files = System.IO.Path.GetFileName(ShopDocuments.FileName);
                        string physicalPath = Server.MapPath("~/Images/" + files);
                        ShopDocuments.SaveAs(physicalPath);
                        sellerDetails.ShopDocuments = files;
                    }

                    //BookSharingEntities db = new BookSharingEntities();
                    db.SellerDetails.Add(sellerDetails);
                    db.SaveChanges();
                    return RedirectToAction("UserDash", "Users");
                }

            }
            ViewBag.Error("The Shop Allready register");
            return View();
        }

        public ActionResult Delete(int id)
        {
            BookSharingEntities db = new BookSharingEntities();
            var sellerDetails = (from x in db.SellerDetails
                                 where x.Id.Equals(id)
                                 select x).ToList();
            foreach (var se in sellerDetails)
            {
                db.SellerDetails.Remove(se);
            }

            return RedirectToAction("UserList", "User");
        }

        public ActionResult SellerDash()
        {
            bool flag = false;
            int id = System.Convert.ToInt32(Session["userId"].ToString());
            BookSharingEntities db = new BookSharingEntities();
            var check = (from x in db.Shops
                         where x.UserId.Equals(id)
                         select x).FirstOrDefault();
            if (check != null) flag = true;
            ViewBag.ShopChecking = flag;
            return View();
        }
        [HttpGet]
        public ActionResult OrderViewForSeller(string search)
        {
            int id = System.Convert.ToInt32(Session["userId"].ToString());
            BookSharingEntities db = new BookSharingEntities();
            List<int> orderIds= (from x in db.OrderDetails
                      where x.SellerId.Equals(id)
                      select x.OrderId).ToList();

            List<Order> orders = new List<Order>();

            if(search == null || search == "")
            {
                foreach (int d in orderIds)
                {
                    var order = (from x in db.Orders
                                 where x.Id.Equals(d)
                                 select x).FirstOrDefault();
                    if(order!= null) orders.Add(order);
                }

            }
            else
            {
                foreach (int d in orderIds)
                {
                    var order = (from x in db.Orders
                                 where x.Id.Equals(d) &&
                                 x.Status.Equals(search)
                                 select x).FirstOrDefault();
                    if (order != null)  orders.Add(order);
                }
            }
            


            var config = new MapperConfiguration(cfg => cfg.CreateMap<Book, BookModel>());
            var mapper = new Mapper(config);

            var config1 = new MapperConfiguration(cfg => cfg.CreateMap<Shop, ShopModel>());
            var mapper1 = new Mapper(config1);

            var config2 = new MapperConfiguration(cfg => cfg.CreateMap<User, UserModel>());
            var mapper2 = new Mapper(config2);

            List<OrderOrderDetailsModel> oods = new List<OrderOrderDetailsModel>();
            foreach (var order in orders)
            {
                List<OrderDetailsModel> odrDetails = new List<OrderDetailsModel>();
                foreach (var x in order.OrderDetails)
                {
                    if (x.SellerId == id)
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



    }
}