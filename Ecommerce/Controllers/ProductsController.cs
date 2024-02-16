using Ecommerce.Controllers.Dto;
using Ecommerce.Controllers.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Xml.Linq;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext context;

        public ProductsController(AppDbContext _context)
        {
            context = _context;
        }
        // All the Crud Opertaions
        [HttpGet]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize]
        public IActionResult GetAll()
        {

           var Productslist =context.Products.Include(x=>x.Category).ToList();

            if (Productslist.IsNullOrEmpty())
            {
                return BadRequest("no products found");


            }
            else
            {
                List<ProductsDto> products = new List<ProductsDto>();
                foreach (var prod in Productslist)
                {

                    products.Add(new ProductsDto
                    {


                        Id = prod.Id,
                        Name = prod.Name
                        ,
                        Catid = prod.Catid,
                        Price = prod.Price
                      
                       
                        ,
                        categoryname = prod.Category.Name


                    });

                }

                return Ok(products);
            }
        }

        [HttpGet("{id:int}",Name ="getbyid")]
        public IActionResult GetByid(int id )
        {

            var Product = context.Products.Include(x=>x.Category)
                .FirstOrDefault(x => x.Id == id);

            if (Product != null)
            {
                var proddto = new ProductsDto()
                {

                    Id = Product.Id,
                    Name = Product.Name
                    ,
                    Catid = Product.Catid,
                    Price = Product.Price
                   , categoryname = Product.Category.Name

                };

                return Ok(proddto);

            }
            return BadRequest("the product not found ");


        }
        [HttpPut]
        public IActionResult Addproduct(ProductstobeaddedDto product)
        {

            if (ModelState.IsValid)
            {

                var producttobeadded = new Products();
                producttobeadded.Catid = product.Catid;
                producttobeadded.Name = product.Name;
                producttobeadded.Price = product.Price;
                producttobeadded.Quantity = product.Quantity;
                producttobeadded.Details = product.Details;



                context.Products.Add(producttobeadded);
                context.SaveChanges();
                var url = Url.Link("getbyid",new{id=producttobeadded.Id });

                return Created(url, producttobeadded);

            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var producttobedeleted = context.Products.FirstOrDefault(x => x.Id == id);

            if (producttobedeleted != null)
            {

                context.Products.Remove(producttobedeleted);
                context.SaveChanges();
                return Ok("the product is removed successfully");

            }
            else return BadRequest("there is no product with this id ");



        }

        [HttpPut("{id:int}")]
        public IActionResult Updateoneproduct(int id, ProductstobeaddedDto product)
        {
            var oldproduct = context.Products.FirstOrDefault(x => x.Id == id);
            if (oldproduct != null)
            {

                if (ModelState.IsValid)
                {
                    oldproduct.Name = product.Name;
                    oldproduct.Price = product.Price;
                    oldproduct.Quantity = product.Quantity;
                    oldproduct.Catid = product.Catid;

                    context.SaveChanges();

                    return Ok("updated successfully");

                }

                else return StatusCode(600, "cant add this object to the databasee ");
            }
            else return BadRequest("no item with this id found");

        }
        [HttpGet("bycategory/{id:int}", Name = "getbycategory")]
        public IActionResult Getproductsbycatid(int id)
        {
            var productsofcategory = context.Products.Where(x => x.Catid == id).ToList();

            if (productsofcategory == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok(productsofcategory);
            }
        }



        [HttpGet("{Name:alpha}")]
        public IActionResult searchforproduct(string Name)
        {

            var productfound=context.Products.FirstOrDefault(x=>x.Name == Name);
            if(productfound != null)
            {
                ProductsDto tobesent = new ProductsDto()
                {
                Name = Name,
                Price = productfound.Price,
                Catid= productfound.Catid,
                };

                return Ok(tobesent);

            }
            else
            {
                return StatusCode(700, "cant find this item in the products");

            }


        }

    }
}
