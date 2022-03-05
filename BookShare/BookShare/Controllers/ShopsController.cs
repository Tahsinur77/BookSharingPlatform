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
    public class ShopsController : Controller
    {
        // GET: Shop
        [HttpGet]
        public ActionResult ShopRegister()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ShopRegister(ShopModel shopModel)
        {
            if (ModelState.IsValid)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<ShopModel,Shop >());
                var mapper = new Mapper(config);
                var shop = mapper.Map<Shop>(shopModel);

                BookSharingEntities db = new BookSharingEntities();
                db.Shops.Add(shop);
                db.SaveChanges();
            }
            return View();
        }
    }
}