using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ForexMasters_site.Models.ViewModels
{
    public class TopicViewModel
    {
        [Required(ErrorMessage = "Please specify topic name")]
        [DisplayName("Topic Name:")]
        public string TopicName { get; set; }

        [DisplayName("Topic Description:")]
        public string TopicDescription { get; set; }

        [Required(ErrorMessage = "A Topic Must Be Associated With A Category!")]
        [DisplayName("Category:")]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "A topic must have at least one presentation video!")]
        [DisplayName("Video(s)")]
        public List<IFormFile> Videos { get; set; }

        [DisplayName("Document(s)")]
        public List<IFormFile> Documents { get; set; }

        [Required(ErrorMessage = "Please enter a password for this topic")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords must match")]
        [DisplayName("Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
