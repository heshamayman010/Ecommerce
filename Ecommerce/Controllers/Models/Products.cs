using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Controllers.Models
{
    public class Products
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Details { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public Category Category { get; set; }
        
        [ForeignKey("Category")]
        public int Catid { get; set; }
        


    }
}
