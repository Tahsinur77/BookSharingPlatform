using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookShare.Models.Entities
{
    public class UserModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please fill your name")]
        [StringLength(50, ErrorMessage = "Name must not excced 50")]
        public string Name { get; set; }
        [Required]
        public string Role { get; set; }
        public string Picture { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
    }
}