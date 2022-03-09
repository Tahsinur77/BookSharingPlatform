using BookShare.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookShare.Models.Entities
{
    public class BookAuthorModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AuthorId { get; set; }
        public string Price { get; set; }
        public string Edition { get; set; }
        public string Publisher { get; set; }
        public string Category { get; set; }
        public string Country { get; set; }
        public string Language { get; set; }
        public string NumberOfPages { get; set; }
        public string Picture { get; set; }
        public string ISBN { get; set; }
        public string Status { get; set; }

        public Author Author { get; set; }
    }
}