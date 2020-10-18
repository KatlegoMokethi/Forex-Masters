using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace ForexMasters_site.Models.ViewModels
{
    public class FlashcardViewModel
    {
        [Required(ErrorMessage = "Please specify flashcard name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please specify flashcard date")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [FileExtensions(Extensions = "jpg,jpeg,png", ErrorMessage = "Only .jpg, .jpeg and .png formats are accepted!")]
        public IFormFile PictureFile { get; set; }
    }
}
