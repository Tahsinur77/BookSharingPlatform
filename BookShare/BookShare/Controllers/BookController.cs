using AutoMapper;
using BookShare.Auth;
using BookShare.Models.Database;
using BookShare.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

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

        public ActionResult AllBookList()
        {
            BookSharingEntities db = new BookSharingEntities();
            var list = db.Books.ToList();

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Book, BookAuthorModel>());
            var mapper = new Mapper(config);
            var listModel = mapper.Map<List<BookAuthorModel>>(list);

            return View(listModel);
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

        [HttpGet]
        public ActionResult ApprovedBookList()
        {
            BookSharingEntities db = new BookSharingEntities();
            var list = (from x in db.Books
                        where x.Status.Equals("Approved")
                        select x).ToList();

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Book, BookAuthorModel>());
            var mapper = new Mapper(config);
            var listModel = mapper.Map<List<BookAuthorModel>>(list);

            string u = Session["user"].ToString();
            var user  = new JavaScriptSerializer().Deserialize<UserModel>(u);

            ViewBag.UserRole = user.Role;
            if(user.Role == "Seller")
            {
                ViewBag.ShopId = (from shopId in db.Shops
                                  where shopId.UserId.Equals(user.Id)
                                  select shopId.Id).FirstOrDefault();
            }

            return View(listModel);
        }

        [HttpGet]
        public ActionResult DenyBookList()
        {
            BookSharingEntities db = new BookSharingEntities();
            var list = (from x in db.Books
                        where x.Status.Equals("Deny")
                        select x).ToList();

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Book, BookAuthorModel>());
            var mapper = new Mapper(config);
            var listModel = mapper.Map<List<BookAuthorModel>>(list);

            return View(listModel);
        }

        [AdminValidation]
        public ActionResult ApproveBook(int id)
        {
            BookSharingEntities db = new BookSharingEntities();
            var oldBook = (from x in db.Books
                           where x.Id.Equals(id) &&
                           x.Status.Equals("Pending")
                           select x).FirstOrDefault();
            Book newBook = new Book();
            newBook.Id = oldBook.Id;
            newBook.Name = oldBook.Name;
            newBook.AuthorId = oldBook.AuthorId;
            newBook.Price = oldBook.Price;
            newBook.Edition = oldBook.Edition;
            newBook.Publisher = oldBook.Publisher;
            newBook.Category = oldBook.Category;
            newBook.Country = oldBook.Country;
            newBook.Language = oldBook.Language;
            newBook.NumberOfPages = oldBook.NumberOfPages;
            newBook.Picture = oldBook.Picture;
            newBook.ISBN = oldBook.ISBN;
            newBook.Status = "Approved";

            db.Entry(oldBook).CurrentValues.SetValues(newBook);
            db.SaveChanges();

            return RedirectToAction("ApprovedBookList");
        }

        [AdminValidation]
        public ActionResult DenyBook(int id)
        {
            BookSharingEntities db = new BookSharingEntities();
            var oldBook = (from x in db.Books
                           where x.Id.Equals(id) &&
                           x.Status.Equals("Pending")
                           select x).FirstOrDefault();
            Book newBook = new Book();
            newBook.Id = oldBook.Id;
            newBook.Name = oldBook.Name;
            newBook.AuthorId = oldBook.AuthorId;
            newBook.Price = oldBook.Price;
            newBook.Edition = oldBook.Edition;
            newBook.Publisher = oldBook.Publisher;
            newBook.Category = oldBook.Category;
            newBook.Country = oldBook.Country;
            newBook.Language = oldBook.Language;
            newBook.NumberOfPages = oldBook.NumberOfPages;
            newBook.Picture = oldBook.Picture;
            newBook.ISBN = oldBook.ISBN;
            newBook.Status = "Deny";

            db.Entry(oldBook).CurrentValues.SetValues(newBook);
            db.SaveChanges();

            return RedirectToAction("DenyBookList");
        }
        [HttpPost]
        public ActionResult BookAddToShop(BookDetailModel bookDetailModel)
        {
            if (ModelState.IsValid)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<BookDetailModel, BookDetail>());
                var mapper = new Mapper(config);
                var bookDetail = mapper.Map<BookDetail>(bookDetailModel);

                BookSharingEntities db = new BookSharingEntities();
                db.BookDetails.Add(bookDetail);
                db.SaveChanges();
                return RedirectToAction("ShopBookList");
            }
            TempData["QuantityError"] = "Quantity not valid";
            return RedirectToAction("ApprovedBookList");
        }

        public ActionResult ShopBookList()
        {
            return View();
        }
    }
}