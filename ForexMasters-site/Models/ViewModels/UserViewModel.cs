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
        [Required(ErrorMessage = "Enter a name")]
        public string Name { get; set; }
        
        public string Surname { get; set; }
        
        public string Country { get; set; }
        
        [Required(ErrorMessage = "Enter E-mail address")]
        [DisplayName("E-mail")]
        [EmailAddress]
        public string Email { get; set; }

        public string Picture { get; set; }

        [Required(ErrorMessage = "Enter Password")]
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
