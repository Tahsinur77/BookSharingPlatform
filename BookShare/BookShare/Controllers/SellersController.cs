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

                if(check == null)
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<SellerDetailsModel, SellerDetail>());
                    var mapper = new Mapper(config);
                    var sellerDetails = mapper.Map<SellerDetail>(sellerDetailsModel);

                    if (ShopDocuments != null)
                    {
                        
                        string files = System.IO.Path.GetFileName(ShopDocuments.FileName);
                        string physicalPath = Server.MapPath("~/Images/"+ files);
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
    }
}