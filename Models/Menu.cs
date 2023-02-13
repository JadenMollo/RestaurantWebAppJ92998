using System.ComponentModel.DataAnnotations;

namespace webapp.Models
{
    public class Menu
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public String Name { get; set; }
        [Required]
        public String Price { get; set; }
        public String Description { get; set; }

    }
}