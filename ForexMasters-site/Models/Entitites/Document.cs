using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForexMasters_site.Models.Entities
{
    public class Document
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DocumentID { get; set; }

        public string FileName { get; set; }
        
        public string FilePath { get; set; }

        public int TopicID { get; set; }
        public Topic Topic { get; set; }
    }
}
