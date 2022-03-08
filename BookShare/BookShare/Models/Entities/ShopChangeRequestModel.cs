﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookShare.Models.Entities
{
    public class ShopChangeRequestModel
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string ShopNumber { get; set; }
        [Required]
        public string ShopDocuments { get; set; }
        [Required]
        public string Location { get; set; }
        public string Status { get; set; }
    }
}