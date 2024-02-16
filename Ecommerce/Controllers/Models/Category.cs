using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Controllers.Models
{
    public class Category
    {

        public int Id { get; set; }
        public string? Name { get; set; }

        public List<Products> Products { get; set; }=new List<Products>();



    }
}





