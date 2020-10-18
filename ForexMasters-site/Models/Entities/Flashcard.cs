using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForexMasters_site.Models.Entities
{
    public class Flashcard
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FlashcardID { get; set; }

        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public string PictureURL { get; set; }
    }
}
