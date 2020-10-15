using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForexMasters_site.Models.Entities
{
    public class Video
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VideoID { get; set; }

        public string VideoName { get; set; }
        
        public string VideoPath { get; set; }

        public int TopicID { get; set; }
        public Topic Topic { get; set; }
    }
}
