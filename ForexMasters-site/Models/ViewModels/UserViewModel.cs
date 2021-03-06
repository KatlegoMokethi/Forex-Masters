﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ForexMasters_site.Models.ViewModels
{
    public class LoginModel
    {
        [Required]
        [UIHint("email")]
        [DisplayName("E-mail")]
        public string Email { get; set; }
        
        [Required]
        [UIHint("password")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; } = "/";
    }

    public class RegisterModel
    {
        [Required(ErrorMessage = "User name is required!")]
        public string Name { get; set; }
        
        public string Surname { get; set; }

        [Required(ErrorMessage = "Phone number is required!")]
        [DisplayName("Phone Number")]
        [Phone]
        public string PhoneNumber { get; set; }
        
        [Required(ErrorMessage = "E-mail address is required!")]
        [DisplayName("E-mail")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Country of origin is required!")]
        public string Country { get; set; }

        [FileExtensions(Extensions = "jpg,jpeg,png")]
        public IFormFile PictureFile { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "The {0} must be at least between {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords must match")]
        [DisplayName("Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }

    public class RoleEditModel
    {
        public IdentityRole Role { get; set; }
        
        public IEnumerable<IdentityUser> Members { get; set; }
        
        public IEnumerable<IdentityUser> NonMembers { get; set; }
    }

    public class RoleModificationModel
    {
        [Required]
        public string RoleName { get; set; }
        
        public string RoleId { get; set; }
        
        public string[] IdsToAdd { get; set; }
        
        public string[] IdsToDelete { get; set; }
    }
}
