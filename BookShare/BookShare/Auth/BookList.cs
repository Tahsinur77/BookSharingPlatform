using AutoMapper;
using BookShare.Models.Database;
using BookShare.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookShare.Auth
{
    public class BookList
    {
        public List<BookDetailBookShopUserModel> shopBookList()
        {
            BookSharingEntities db = new BookSharingEntities();
            var books = db.BookDetails.ToList();

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Book, BookAuthorModel>());
            var mapper = new Mapper(config);
            var config1 = new MapperConfiguration(cfg => cfg.CreateMap<Shop, ShopModel>());
            var mapper1 = new Mapper(config1);
            var config2 = new MapperConfiguration(cfg => cfg.CreateMap<User, UserModel>());
            var mapper2 = new Mapper(config2);

            List<BookDetailBookShopUserModel> onkGulass = new List<BookDetailBookShopUserModel>();

            foreach (var book in books)
            {

                var bookModel = mapper.Map<BookAuthorModel>(book.Book);
                var shopModel = mapper1.Map<ShopModel>(book.Shop);
                var userModel = mapper2.Map<UserModel>(book.User);

                BookDetailBookShopUserModel onkGula = new BookDetailBookShopUserModel();
                onkGula.SellerId = book.User.Id;
                onkGula.ShopId = book.ShopId;
                onkGula.BookId = book.BookId;
                onkGula.BookQuantity = book.BookQuantity;
                onkGula.Book = bookModel;
                onkGula.Shop = shopModel;
                onkGula.User = userModel;

                onkGulass.Add(onkGula);
            }

            return onkGulass;
        }

        public BookDetailBookShopUserModel bookDetails(int id)
        {
            BookSharingEntities db = new BookSharingEntities();
            var book = (from x in db.BookDetails
                         where x.BookId.Equals(id)
                         select x).FirstOrDefault();

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Book, BookAuthorModel>());
            var mapper = new Mapper(config);
            var config1 = new MapperConfiguration(cfg => cfg.CreateMap<Shop, ShopModel>());
            var mapper1 = new Mapper(config1);
            var config2 = new MapperConfiguration(cfg => cfg.CreateMap<User, UserModel>());
            var mapper2 = new Mapper(config2);

            var bookModel = mapper.Map<BookAuthorModel>(book.Book);
            var shopModel = mapper1.Map<ShopModel>(book.Shop);
            var userModel = mapper2.Map<UserModel>(book.User);

            BookDetailBookShopUserModel onkGula = new BookDetailBookShopUserModel();
            onkGula.SellerId = book.User.Id;
            onkGula.ShopId = book.ShopId;
            onkGula.BookId = book.BookId;
            onkGula.BookQuantity = book.BookQuantity;
            onkGula.Book = bookModel;
            onkGula.Shop = shopModel;
            onkGula.User = userModel;

            return onkGula;
        }
    }
}