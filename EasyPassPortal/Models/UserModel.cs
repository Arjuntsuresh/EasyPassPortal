﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EasyPassPortal.Models
{
    public class UserModel
        
    {
        [Display (Name ="User ID")]
        public int UserID { get; set; }
        [Display (Name="Email")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Display (Name ="Password")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}