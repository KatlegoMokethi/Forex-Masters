using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ForexMasters_site.Models.ViewModels
{
    public class PDFViewModel
    {
        [Required(ErrorMessage = "Must upload at least one document file!")]
        [DisplayName("Document(s)")]
        public List<IFormFile> Documents { get; set; }
    }
}
