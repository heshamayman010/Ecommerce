using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Controllers.Models
{
    public class Appuser:IdentityUser
    {

        public String? Address { get; set; }


    }
}
