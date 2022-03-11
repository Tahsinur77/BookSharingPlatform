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

        [HttpGet]
        public ActionResult AllRequestedList(string searchNid,string searchShopNumber,string searchStatus)
        {
            int check = 0;
            if (searchNid != "") check++;
            if (searchShopNumber != "") check++;
            if (searchStatus != "") check++;
            BookSharingEntities db = new BookSharingEntities();
            List<SellerDetail> list;

            if ((searchNid == null && searchShopNumber == null && searchStatus ==null) || check==0)
            {
                list = db.SellerDetails.ToList();
            }
            else if(check == 1)
            {
                list = (from x in db.SellerDetails
                        where x.ShopNumber.Equals(searchShopNumber) ||
                        x.Nid.Equals(searchNid) ||
                        x.Status.Equals(searchStatus)
                        select x).ToList();
            }
            else if(check == 2)
            {
                if (searchNid != "" && searchShopNumber != "")
                {
                    list = (from x in db.SellerDetails
                            where x.ShopNumber.Equals(searchShopNumber)
                            && x.Nid.Equals(searchNid)
                            select x).ToList();
                }
                else if (searchNid != "" && searchStatus != "")
                {
                    list = (from x in db.SellerDetails
                            where x.Status.Equals(searchStatus)
                            && x.Nid.Equals(searchNid)
                            select x).ToList();
                }
                else
                {
                    list = (from x in db.SellerDetails
                            where x.Status.Equals(searchStatus)
                            && x.ShopNumber.Equals(searchShopNumber)
                            select x).ToList();
                }
            }
            else
            {
                list = (from x in db.SellerDetails
                        where x.ShopNumber.Equals(searchShopNumber) &&
                        x.Nid.Equals(searchNid) &&
                        x.Status.Equals(searchStatus)
                        select x).ToList();
            }



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
            if (fileName == null)
            {
                fileName = (from y in db.ShopChangeRequests
                            where y.UserId.Equals(id)
                            select y.ShopDocuments).FirstOrDefault();
            }
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
                                    && y.Status.Equals("painding")
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

        public ActionResult Deny(int id)
        {
            BookSharingEntities db = new BookSharingEntities();
            var oldSellerDetails = (from y in db.SellerDetails
                                    where y.SellerId.Equals(id)
                                    && y.Status.Equals("painding")
                                    select y).FirstOrDefault();
            SellerDetail se = new SellerDetail();
            se.Id = oldSellerDetails.Id;
            se.SellerId = oldSellerDetails.SellerId;
            se.Nid = oldSellerDetails.Nid;
            se.Status = "Deny";
            se.ShopNumber = oldSellerDetails.ShopNumber;
            se.ShopDocuments = oldSellerDetails.ShopDocuments;
            db.Entry(oldSellerDetails).CurrentValues.SetValues(se);
            db.SaveChanges();

            return RedirectToAction("SellerPaindingList");
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

        public ActionResult SellerDetails(int id)
        {
            BookSharingEntities db = new BookSharingEntities();
            var sellerDetails = (from x in db.SellerDetails
                                 where x.SellerId.Equals(id)
                                 select x).FirstOrDefault();

            var config = new MapperConfiguration(cfg => cfg.CreateMap<SellerDetail, SellerDetailsModel>());
            var mapper = new Mapper(config);
            var sellerDetailsModel = mapper.Map<SellerDetailsModel>(sellerDetails);

            return View(sellerDetailsModel);
        }

        [HttpGet]
        public ActionResult ChangingShopRequest()
        {
            BookSharingEntities db = new BookSharingEntities();
            var list = (from x in db.ShopChangeRequests
                        where x.Status.Equals("painding")
                        select x).ToList();

            var config = new MapperConfiguration(cfg => cfg.CreateMap<ShopChangeRequest, ShopChangeRequestUsersModel>());
            var mapper = new Mapper(config);
            var listModel = mapper.Map<List<ShopChangeRequestUsersModel>>(list);
                        
            return View(listModel);
        }

        public ActionResult ShopChangeApprove(int id)
        {
            BookSharingEntities db = new BookSharingEntities();
            var oldShopChangeReq = (from x in db.ShopChangeRequests
                                    where x.UserId.Equals(id)
                                    select x).FirstOrDefault();
            var newShopChangeReq = new ShopChangeRequest();
            newShopChangeReq.Id = oldShopChangeReq.Id;
            newShopChangeReq.UserId = oldShopChangeReq.UserId;
            newShopChangeReq.ShopNumber = oldShopChangeReq.ShopNumber;
            newShopChangeReq.ShopDocuments = oldShopChangeReq.ShopDocuments;
            newShopChangeReq.Name = oldShopChangeReq.Name;
            newShopChangeReq.Location = oldShopChangeReq.Location;
            newShopChangeReq.Status = "Approved";
            
            


            var oldSellerDetails = (from y in db.SellerDetails
                                    where y.SellerId.Equals(id) && 
                                    y.Status.Equals("Approved")
                                    select y).FirstOrDefault();
            var newSellerDetails = new SellerDetail();
            newSellerDetails.Id = oldSellerDetails.Id;
            newSellerDetails.SellerId = oldSellerDetails.SellerId;
            newSellerDetails.Nid = oldSellerDetails.Nid;
            newSellerDetails.ShopDocuments = oldShopChangeReq.ShopDocuments;
            newSellerDetails.ShopNumber = oldShopChangeReq.ShopNumber;
            newSellerDetails.Status = oldSellerDetails.Status;

            var oldShop = (from z in db.Shops
                           where z.UserId.Equals(id)
                           select z).FirstOrDefault();
            var newShop = new Shop();
            newShop.Id = oldShop.Id;
            newShop.Name = oldShopChangeReq.Name;
            newShop.Location = oldShopChangeReq.Location;
            newShop.UserId = oldShop.UserId;
            newShop.ShopNumber = oldShopChangeReq.ShopNumber;


            db.Entry(oldShopChangeReq).CurrentValues.SetValues(newShopChangeReq);
            db.Entry(oldSellerDetails).CurrentValues.SetValues(newSellerDetails);
            db.Entry(oldShop).CurrentValues.SetValues(newShop);
            db.SaveChanges();


            return RedirectToAction("AdminDash");
        }

        public ActionResult ShopChangeDeny(int id)
        {
            BookSharingEntities db = new BookSharingEntities();
            var oldShopChangeReq = (from x in db.ShopChangeRequests
                                    where x.UserId.Equals(id)
                                    select x).FirstOrDefault();
            var newShopChangeReq = new ShopChangeRequest();
            newShopChangeReq.Id = oldShopChangeReq.Id;
            newShopChangeReq.UserId = oldShopChangeReq.UserId;
            newShopChangeReq.ShopNumber = oldShopChangeReq.ShopNumber;
            newShopChangeReq.ShopDocuments = oldShopChangeReq.ShopDocuments;
            newShopChangeReq.Name = oldShopChangeReq.Name;
            newShopChangeReq.Location = oldShopChangeReq.Location;
            newShopChangeReq.Status = "Deny";
            db.Entry(oldShopChangeReq).CurrentValues.SetValues(newShopChangeReq);
            db.SaveChanges();
            return RedirectToAction("AdminDash");
        }


        public ActionResult AuthorlistForAdmin()
        {
            BookSharingEntities db = new BookSharingEntities();
            var list = db.Authors.ToList();

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Author, AuthorModel>());
            var mapper = new Mapper(config);
            var listModel = mapper.Map<List<AuthorModel>>(list);

            return View(listModel);
        }


        public ActionResult AuthorDelete(int id)
        {
            BookSharingEntities db = new BookSharingEntities();
            var author = (from x in db.Authors
                          where x.Id.Equals(id)
                          select x).FirstOrDefault();
            db.Authors.Remove(author);
            db.SaveChanges();
            return RedirectToAction("AuthorlistForAdmin");
        }


        [HttpGet]
        public ActionResult AuthorEdit(int id)
        {
            BookSharingEntities db = new BookSharingEntities();
            var author = (from a in db.Authors
                        where a.Id.Equals(id)
                        select a).FirstOrDefault();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Author, AuthorModel>()); ;
            var mapper = new Mapper(config);
            var authorModel = mapper.Map<AuthorModel>(author);

            return View(authorModel);
        }

        [HttpPost]

        public ActionResult AuthorEdit(AuthorModel author, HttpPostedFileBase Picture)
        {

            if (ModelState.IsValid)
            {
                BookSharingEntities db = new BookSharingEntities();
                var pic = (from b in db.Authors
                           where b.Id.Equals(author.Id)
                           select b).FirstOrDefault();
                string ImageName = pic.Picture;
                if (Picture != null)
                {
                    ImageName = System.IO.Path.GetFileName(Picture.FileName);
                    string physicalPath = Server.MapPath("~/Images/" + ImageName);
                    Picture.SaveAs(physicalPath);
                }

                Author a = new Author();
                a.Id = author.Id;
                a.Name = author.Name;
                a.Email = author.Email;
                
                a.Picture = ImageName;
                a.Details = author.Details;


                db.Entry(pic).CurrentValues.SetValues(a);
                db.SaveChanges();

                return RedirectToAction("AuthorList");
            }
            return View(author);
        }

    }
}