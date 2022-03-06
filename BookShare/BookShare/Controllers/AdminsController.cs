using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AutoMapper;
using BookShare.Auth;
using BookShare.Models.Database;
using BookShare.Models.Entities;

namespace BookShare.Controllers
{
    [AdminValidation]
    public class AdminsController : Controller
    {
        // GET: Admins
        public ActionResult AdminDash()
        {
            return View();
        }

        public ActionResult SellerPaindingList()
        {
            BookSharingEntities db = new BookSharingEntities();
            var list = (from x in db.SellerDetails
                        where x.Status.Equals("painding")
                        select x).ToList();

            var config = new MapperConfiguration(cfg => cfg.CreateMap<SellerDetail, SellerDetailsUserModel>());
            var mapper = new Mapper(config);
            var sellerDetailsUserModel = mapper.Map<List<SellerDetailsUserModel>>(list);

            return View(sellerDetailsUserModel);
        }

        public void Download(int id)
        {
            BookSharingEntities db = new BookSharingEntities();
            var fileName = (from x in db.SellerDetails
                            where x.Id.Equals(id)
                            select x.ShopDocuments).FirstOrDefault();
            string FilePath = Server.MapPath("~/Images/"+fileName);
            WebClient User = new WebClient();
            Byte[] FileBuffer = User.DownloadData(FilePath);
            if (FileBuffer != null)
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-length", FileBuffer.Length.ToString());
                Response.BinaryWrite(FileBuffer);
            }
        }

        public ActionResult Approve(int id)
        {
           
            BookSharingEntities db = new BookSharingEntities();

            var oldSellerDetails = (from y in db.SellerDetails
                                    where y.SellerId.Equals(id)
                                    select y).FirstOrDefault();
            SellerDetail se = new SellerDetail();
            se.Id = oldSellerDetails.Id;
            se.SellerId = oldSellerDetails.SellerId;
            se.Nid = oldSellerDetails.Nid;
            se.Status = "Approved";
            se.ShopNumber = oldSellerDetails.ShopNumber;
            se.ShopDocuments = oldSellerDetails.ShopDocuments;

            var old = (from x in db.Users
                       where x.Id.Equals(id)
                       select x).FirstOrDefault();

            User u = new User();
            u.Id = old.Id;
            u.Name = old.Name;
            u.Email = old.Email;
            u.Phone = old.Phone;
            u.Address = old.Address;
            u.Role = "Seller";
            u.Picture = old.Picture;
            u.Password = old.Password;

            db.Entry(old).CurrentValues.SetValues(u);
            db.Entry(oldSellerDetails).CurrentValues.SetValues(se);
            db.SaveChanges();



            return RedirectToAction("SellerList");
        }

        public ActionResult SellerList()
        {
            //userSeller model diye kaj kora lagbe
            BookSharingEntities db = new BookSharingEntities();
            var list = (from x in db.Users
                        where x.Role.Equals("Seller")
                        select x).ToList();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<User, UserModel>());
            var mapper = new Mapper(config);
            var listModel = mapper.Map<List<UserModel>>(list);

            return View(listModel);
        }

        
    }
}