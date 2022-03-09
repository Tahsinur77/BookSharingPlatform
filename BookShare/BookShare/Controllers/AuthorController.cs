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
    public class AuthorController : Controller
    {
        // GET: Author
        [HttpGet]
        public ActionResult CreateAuthor()
        {
            return View(new AuthorModel());
        }
        [HttpPost]
        public ActionResult CreateAuthor(AuthorModel authorModel,HttpPostedFileBase Picture)
        {
            if (ModelState.IsValid)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<AuthorModel, Author>());
                var mapper = new Mapper(config);
                var author = mapper.Map<Author>(authorModel);

                string ImageName = System.IO.Path.GetFileName(Picture.FileName);
                string physicalPath = Server.MapPath("~/Images/" + ImageName);
                Picture.SaveAs(physicalPath);

                if (authorModel.Email == "" ||authorModel.Email == null) author.Email = "unknown";
                author.Picture = ImageName;

                BookSharingEntities db = new BookSharingEntities();
                db.Authors.Add(author);
                db.SaveChanges();
                return RedirectToAction("Authorlist");
            }
            return View(authorModel);
        }
        [HttpGet]
        public ActionResult Authorlist()
        {
            BookSharingEntities db = new BookSharingEntities();
            var list = db.Authors.ToList();

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Author, AuthorModel>());
            var mapper = new Mapper(config);
            var listModel = mapper.Map<List<AuthorModel>>(list);

            return View(listModel);
        }


    }
}