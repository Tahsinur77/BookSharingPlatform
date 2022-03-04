using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookShare.Controllers
{
    public class SellersController : Controller
    {
        // GET: Sellers
        
        public ActionResult SellerRegistration()
        {
            return View();
        }
    }
}