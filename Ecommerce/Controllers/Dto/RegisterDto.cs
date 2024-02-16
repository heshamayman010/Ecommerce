using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Controllers.Dto
{
    public class RegisterDto
    {

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string VerifyPassword { get; set; }



    }
}
