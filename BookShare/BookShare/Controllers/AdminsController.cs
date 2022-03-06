using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
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

        public void Download(int id)
        {
            BookSharingEntities db = new BookSharingEntities();
            var fileName = (from x in db.SellerDetails
                            where x.Id.Equals(id)
                            select x.ShopDocuments).FirstOrDefault();
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
    }
}