using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EasyPassPortal.Models
{
    public class UserPassportDetails
    {
        [Display(Name = "Account ID")]
        public int id { get; set; }

        [Display(Name = "Full name")]
        public string FullName { get; set; }

        [Display(Name = "Father name")]
        public string FatherName { get; set; }

        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Display(Name = "Date of birth")]
        public string DateOfBirth { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Religion")]
        public string Religion { get; set; }

        [Display(Name = "State")]
        public string State { get; set; }

        [Display(Name = "Nationality")]
        public string Nationality { get; set; }

        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}