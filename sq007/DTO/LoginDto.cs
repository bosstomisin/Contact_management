using System.ComponentModel.DataAnnotations;

namespace sq007.Controllers
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
         public bool RememberMe { get; set; }
    }
} 