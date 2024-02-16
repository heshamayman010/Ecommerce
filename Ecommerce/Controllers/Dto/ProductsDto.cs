using Ecommerce.Controllers.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Controllers.Dto
{
    public class ProductsDto
    {

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }
        [Range(minimum:1000,maximum:1000000000)]
        public decimal Price { get; set; }

        [Required]
        public int Catid { get; set; }

        public string?   categoryname { get; set; }




    }
}
