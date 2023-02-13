using System.ComponentModel.DataAnnotations;

namespace webapp.Models
{
    public class Login
    {
        internal bool RememberMe;

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
