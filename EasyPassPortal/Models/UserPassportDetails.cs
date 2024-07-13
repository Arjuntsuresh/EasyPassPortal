using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EasyPassPortal.Models
{
    public class UserPassportDetails
    {
        [Required]
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Full name")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Father name")]
        public string FatherName { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of birth")]
       
        public String DateOfBirth { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Religion")]
        public string Religion { get; set; }

        [Required]
        [Display(Name = "State")]
        public string State { get; set; }

        [Required]
        [Display(Name = "District")]
        public string District { get; set; }


        [Required]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

       
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Aadhar number")]
        public string AadharNumber { get; set; }

        [Required]
        [Display(Name = "Pancard number")]
        public string PancardNumber { get; set; }

        [Required]
        [Display(Name = "Education")]
        public string Education { get; set; }

        public string Status { get; set; }
       
        public byte[] Image { get; set; }
     
        public byte[] AadharPhoto { get; set; }
       
        public byte[] IdProof { get; set; }

        [Required]
        [Display(Name = "Mother name")]
        public string MothersName { get; set; }

        public static ValidationResult ValidateDateOfBirth(DateTime dateOfBirth, ValidationContext context)
        {
            if (dateOfBirth >= DateTime.Now)
            {
                return new ValidationResult("Date of Birth cannot be in the future.");
            }

            int age = DateTime.Now.Year - dateOfBirth.Year;
            if (dateOfBirth > DateTime.Now.AddYears(-age)) age--;

            return age >= 18 ? ValidationResult.Success : new ValidationResult("Age should be 18 or above.");
        }
    }
}