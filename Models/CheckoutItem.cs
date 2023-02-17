using System.ComponentModel.DataAnnotations;

namespace webapp.Models
{
    public class CheckoutItem
    {
        [Key, Required]
        public int ID { get; set; }
        [Required]
        public string ?Price { get; set; }
        [Required, StringLength(50)]
        public string ?Name { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
