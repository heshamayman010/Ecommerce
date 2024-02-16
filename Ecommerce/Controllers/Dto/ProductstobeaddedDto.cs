using Ecommerce.Controllers.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Controllers.Dto
{
    public class ProductstobeaddedDto
    {
        public string Name { get; set; }
        public string? Details { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }


        public int Catid { get; set; }

    }
}
