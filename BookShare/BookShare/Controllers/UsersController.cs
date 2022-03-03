﻿using BookShare.Models.Database;
using BookShare.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;

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
                string ImageName = System.IO.Path.GetFileName(Picture.FileName);
                string physicalPath = Server.MapPath("~/Images/" + ImageName);

                Picture.SaveAs(physicalPath);

                User u = new User();
                u.Name = user.Name;
                u.Email = user.Email;
                u.Phone = user.Phone;
                u.Address = user.Address;
                u.Role = user.Role;
                u.Picture = ImageName;

                BookSharingEntities db = new BookSharingEntities();
                db.Users.Add(u);
                db.SaveChanges();
                return RedirectToAction("UserList");
            }
            

            return View(user);
        }

        [HttpGet]
        public ActionResult UserList()
        {
            BookSharingEntities db = new BookSharingEntities();
            var list = db.Users.ToList();
            var config = new MapperConfiguration(cgf => cgf.CreateMap<User,UserModel>());
            var mapper = new Mapper(config);
            var listModel = mapper.Map<List<UserModel>>(list);
            return View(listModel);
        }

        
        

    }
}