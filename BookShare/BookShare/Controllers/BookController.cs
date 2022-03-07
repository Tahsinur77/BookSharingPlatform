using BookShare.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookShare.Controllers
{
    [SellerOrAdminValidation]
    public class BookController : Controller
    {
        // GET: Book
        
        [HttpGet]
        public ActionResult BookCreate()
        {
            return View();
        }
    }
}