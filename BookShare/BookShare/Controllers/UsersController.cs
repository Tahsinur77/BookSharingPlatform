using BookShare.Models.Database;
using BookShare.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using BookShare.Auth;

namespace BookShare.Controllers
{
    public class UsersController : Controller
    {
        // GET: User

        [HttpGet]
        public ActionResult Create()
        {
            return View(new UserModel());
        }

        [HttpPost]
        public ActionResult Create(UserModel user, HttpPostedFileBase Picture)
        {
            if (ModelState.IsValid)
            {
                string ImageName = null;
                if(Picture != null)
                {
                    ImageName = System.IO.Path.GetFileName(Picture.FileName);
                    string physicalPath = Server.MapPath("~/Images/" + ImageName);

                    Picture.SaveAs(physicalPath);
                }
                

                User u = new User();
                u.Name = user.Name;
                u.Email = user.Email;
                u.Phone = user.Phone;
                u.Address = user.Address;
                u.Role = user.Role;
                u.Picture = ImageName;
                u.Password = user.Password;

                BookSharingEntities db = new BookSharingEntities();
                db.Users.Add(u);
                db.SaveChanges();
                return RedirectToAction("UserList");
            }
            

            return View(user);
        }

        [HttpGet]
        [AdminValidation]
        public ActionResult UserList()
        {
            BookSharingEntities db = new BookSharingEntities();
            var list = db.Users.ToList();
            var config = new MapperConfiguration(cgf => cgf.CreateMap<User,UserModel>());
            var mapper = new Mapper(config);
            var listModel = mapper.Map<List<UserModel>>(list);
            return View(listModel);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            BookSharingEntities db = new BookSharingEntities();
            var user = (from u in db.Users
                        where u.Id.Equals(id)
                        select u).FirstOrDefault();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<User, UserModel>()); ;
            var mapper = new Mapper(config);
            var userModel = mapper.Map<UserModel>(user);

            return View(userModel);

        }

        [HttpPost]
        public ActionResult Edit(UserModel user, HttpPostedFileBase Picture)
        {

            if (ModelState.IsValid)
            {
                BookSharingEntities db = new BookSharingEntities();
                var old = (from b in db.Users
                           where b.Id.Equals(user.Id)
                           select b).FirstOrDefault();
                string ImageName = old.Picture;
                if (Picture != null)
                {
                    ImageName = System.IO.Path.GetFileName(Picture.FileName);
                    string physicalPath = Server.MapPath("~/Images/" + ImageName);
                    Picture.SaveAs(physicalPath);
                }
           
                User u = new User();
                u.Id = user.Id;
                u.Name = user.Name;
                u.Email = user.Email;
                u.Phone = user.Phone;
                u.Address = user.Address;
                u.Role = user.Role;
                u.Picture = ImageName;
                u.Password = user.Password;

                
                db.Entry(old).CurrentValues.SetValues(u);
                db.SaveChanges();

                return RedirectToAction("UserList");
            }
            return View(user);
        }

        public ActionResult Delete(int id)
        {
            BookSharingEntities db = new BookSharingEntities();

            //delete from SellerDetails Table
            var sellerDetails = (from x in db.SellerDetails
                                 where x.SellerId.Equals(id)
                                 select x).ToList();
            foreach (var se in sellerDetails)
            {
                db.SellerDetails.Remove(se);
            }


            //Delete from User Table
            var user = (from u in db.Users
                        where u.Id.Equals(id)
                        select u).FirstOrDefault();
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("UserList");
        }

        [UserValidation]
        public ActionResult UserDash()
        {
            BookList b = new BookList();
            var bookList = b.shopBookList();
            return View(bookList);
        }

        [HttpGet]
        public ActionResult BookDetails(int id)
        {
            BookList b = new BookList();
            var bookDetails = b.bookDetails(id);
            return View(bookDetails);
        }

    }
}