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
    public class BookController : Controller
    {
        // GET: Book
        
        [HttpGet]
        public ActionResult BookCreate(int id)
        {
            ViewBag.AuthorId = id;
            return View();
        }

        [HttpPost]

        public ActionResult BookCreate(BookModel bookModel, HttpPostedFileBase Picture)
        {
            if(ModelState.IsValid)
            {
                string ImageName = System.IO.Path.GetFileName(Picture.FileName);
                string physicalPath = Server.MapPath("~/Images/" + ImageName);
                Picture.SaveAs(physicalPath);

                var config = new MapperConfiguration(cfg => cfg.CreateMap<BookModel, Book>() );
                var mapper = new Mapper(config);
                var book = mapper.Map<Book>(bookModel);

                book.Picture = ImageName;
                BookSharingEntities db = new BookSharingEntities();
                db.Books.Add(book);

                db.SaveChanges();

                return RedirectToAction("SellerDash", "Sellers");

            }

            return View();

        }

        [HttpGet]
        public ActionResult PendingBookList()
        {
            BookSharingEntities db = new BookSharingEntities();
            var list = (from x in db.Books
                        where x.Status.Equals("Pending")
                        select x ).ToList();

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Book, BookAuthorModel>());
            var mapper = new Mapper(config);
            var listModel = mapper.Map<List <BookAuthorModel> >(list);

            return View(listModel);
        }
    }
}