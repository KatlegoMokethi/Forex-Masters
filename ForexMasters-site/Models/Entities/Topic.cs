using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForexMasters_site.Models.Entities
{
    public class Topic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string TopicID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryID { get; set; }
        public ICollection<Video> Videos { get; set; }
        public ICollection<Document> Documents { get; set; }
        public string Password { get; set; }
    }
}
