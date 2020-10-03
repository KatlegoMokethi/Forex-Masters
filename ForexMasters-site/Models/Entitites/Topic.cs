using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForexMasters_site.Models.Entities
{
    public class Topic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TopicID { get; set; }
        
        [Required(ErrorMessage = "Please specify topic name")]
        public string Name { get; set; }
        
        public string Description { get; set; }

        [DisplayName("Category Type")]
        public int CategoryID { get; set; }
        [ForeignKey("CategoryID")]
        public virtual Category Category { get; set; }
        
        [Required(ErrorMessage = "A topic must have at least one presentation video")]
        public ICollection<Video> Videos { get; set; }
        
        public ICollection<Document> Documents { get; set; }
        
        [Required(ErrorMessage = "Please enter a password for this topic")]
        public string Password { get; set; }
    }
}
