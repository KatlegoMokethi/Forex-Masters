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

        public byte[] DocumentFile { get; set; }

        public string FileURL { get; set; }

        public int TopicID { get; set; }
        public Topic Topic { get; set; }
    }
}
