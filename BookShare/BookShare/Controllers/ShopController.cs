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
        [HttpGet]
        public ActionResult RequestChangeShop()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RequestChangeShop(ShopChangeRequestModel shopChangeRequestModel,HttpPostedFileBase ShopDocuments)
        {
            if (ModelState.IsValid)
            {
                string ImageName = null;
                if (ShopDocuments != null)
                {
                    ImageName = System.IO.Path.GetFileName(ShopDocuments.FileName);
                    string physicalPath = Server.MapPath("~/Images/" + ImageName);

                    ShopDocuments.SaveAs(physicalPath);
                }

                ShopChangeRequest shopChangeRequest = new ShopChangeRequest();
                shopChangeRequest.Name = shopChangeRequestModel.Name;
                shopChangeRequest.UserId = shopChangeRequestModel.UserId;
                shopChangeRequest.ShopNumber = shopChangeRequestModel.ShopNumber;
                shopChangeRequest.ShopDocuments = ImageName;
                shopChangeRequest.Location = shopChangeRequestModel.Location;
                shopChangeRequest.Status = shopChangeRequestModel.Status;

                BookSharingEntities db = new BookSharingEntities();
                db.ShopChangeRequests.Add(shopChangeRequest);
                db.SaveChanges();
                return RedirectToAction("SellerDash", "Sellers");
                
            }
            return View(shopChangeRequestModel);
        }
    }
}