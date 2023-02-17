using System.ComponentModel.DataAnnotations;

namespace webapp.Models
{
    public class OrderHistories
    {
        [Key, Required]
        public int OrderNo { get; set; }
        [Required]
        public string? Email { get; set; }
        public int TableNo { get; set; }

    }
}
