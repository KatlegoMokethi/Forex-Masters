using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ForexMasters_site.Models.Entities
{
    public class Flashcard
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FlashcardID { get; set; }

        [Required(ErrorMessage = "Please specify flashcard name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please specify flashcard date")]
        public DateTime Date { get; set; }
        
        public string PicturePath { get; set; }
    }
}
