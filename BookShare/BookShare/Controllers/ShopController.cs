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
    [SellerOrAdminValidation]
    public class ShopController : Controller
    {
        // GET: Shop
        [HttpGet]
        public ActionResult ShopCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ShopCreate(ShopModel shopModel)
        {
            if (ModelState.IsValid)
            {
                int id = System.Convert.ToInt32(Session["userId"].ToString());
                BookSharingEntities db = new BookSharingEntities();
                var shopNumber = (from x in db.SellerDetails
                                  where x.SellerId.Equals(id)
                                  select x.ShopNumber).FirstOrDefault();

                if (shopNumber == shopModel.ShopNumber)
                {

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<ShopModel, Shop>());
                    var mapper = new Mapper(config);
                    var shop = mapper.Map<Shop>(shopModel);


                    db.Shops.Add(shop);
                    db.SaveChanges();
                    TempData["shopAdd"] = "Shop added Successfully";
                    return RedirectToAction("SellerDash", "Sellers");
                }
                ViewBag.ShopNumber = "Check Your Register Shop Number";
                return View();                
            }
            

            return View(shopModel);
        }
    }
}