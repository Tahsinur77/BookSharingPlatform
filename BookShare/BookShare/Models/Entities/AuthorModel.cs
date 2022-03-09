using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookShare.Models.Entities
{
    public class AuthorModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Picture { get; set; }
        [Required]
        public string Details { get; set; }
        public string Email { get; set; }
    }
}