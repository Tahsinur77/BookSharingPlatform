using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookShare.Models.Entities
{
    public class BookModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int AuthorId { get; set; }

        [Required]
        public string Price { get; set; }

        [Required]
        public string Edition { get; set; }

        [Required]
        public string Publisher { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string Language { get; set; }

        [Required]
        public string NumberOfPages { get; set; }

        [Required]
        public string Picture { get; set; }

        [Required]
        public string ISBN { get; set; }

        [Required]
        public string Status { get; set; }
    }
}