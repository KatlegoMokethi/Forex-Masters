using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForexMasters_site.Models.ViewModels
{
    public class UsersViewModel
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        [Column(TypeName = "money")]
        public decimal Balance { get; set; }

        [DisplayName("Active")]
        public bool IsActive { get; set; }

        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        [DisplayName("E-mail")]
        public string Email { get; set; }

        [FileExtensions(Extensions = "jpg,jpeg,png")]
        public IFormFile PictureFile { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
