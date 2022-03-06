using AutoMapper;
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
    public class LoginController : Controller
    {
        // GET: Login
        [HttpGet]
        public ActionResult Login()
        {
            if (Session["user"] != null)
            {
                var jsonUser = Session["user"].ToString();
                var user = new JavaScriptSerializer().Deserialize<UserModel>(jsonUser);
                if (user.Role == "Admin") return RedirectToAction("AdminDash", "Admins");
                else if (user.Role == "User") return RedirectToAction("UserDash", "Users");
                else if (user.Role == "Seller") return RedirectToAction("SellerDash", "Sellers");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(string Email,string Password)
        {
            BookSharingEntities db = new BookSharingEntities();
            var login = (from u in db.Users
                         where u.Email.Equals(Email) &&
                         u.Password.Equals(Password)
                         select u).FirstOrDefault();
            if (login != null)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<User, UserModel>());
                var mapper = new Mapper(config);
                var user = mapper.Map<UserModel>(login);


                string jsonUser = new JavaScriptSerializer().Serialize(user);

                Session["user"] = jsonUser;
                Session["userId"] = user.Id;
                if (user.Role == "Admin") return RedirectToAction("AdminDash", "Admins");
                else if (user.Role == "Seller") return RedirectToAction("SellerDash", "Sellers");
                return RedirectToAction("UserDash", "Users");
            }

            ViewBag.Error = "Invalid";
            return View();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Remove("user");
            Session.Remove("userId");
            return RedirectToAction("Login");
        }

    }
}