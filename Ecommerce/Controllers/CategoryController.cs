using Ecommerce.Controllers.Dto;
using Ecommerce.Controllers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext context;

        public CategoryController(AppDbContext _context)
        {
            context = _context;
        }
        // All the Crud Opertaions
       
        
        [HttpGet]
        public IActionResult GetAll()
        {

            var allcategories=context.Categories.ToList();
            if (allcategories.IsNullOrEmpty())
            {
                return BadRequest("no products found");


            }
            else
            {
                List<CategoryDto> categories = new List<CategoryDto>();
                foreach (var cat in allcategories)
                {

                    categories.Add(new CategoryDto
                    {
                        Id= cat.Id,
                        Name= cat.Name


                    });

                }

                return Ok(categories);
            }
        }

        [HttpGet("{id:int}", Name = "getcatbyid")]
        public IActionResult GetByid(int id)
        {

            var category = context.Categories.FirstOrDefault(c => c.Id == id);

            if (category != null)
            {
                var categdto = new CategoryDto()
                {

                    Id = category.Id,
                    Name = category.Name

                };

                return Ok(categdto);

            }
            return BadRequest("the Category not found ");


        }
        [HttpPut]
        public IActionResult AddCategory(string Name)
        {

            {

                Category newcat = new Category();
                newcat.Name = Name;

                context.Categories.Add(newcat);
                context.SaveChanges();
                var url = Url.Link("getcatbyid", new { id = newcat.Id });

                return Created(url, newcat);


            }
        }
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var categorytobedeleted = context.Categories.FirstOrDefault(x => x.Id == id);

            if (categorytobedeleted != null)
            {

                context.Categories.Remove(categorytobedeleted);
                context.SaveChanges();
                return Ok("the category is removed successfully");

            }
            else return BadRequest("there is no Category with this id ");



        }

        [HttpPut("{id:int}")]
        public IActionResult Updateoneproduct(int id,string newname)
        {
            var oldcat = context.Categories.FirstOrDefault(x => x.Id == id);
            if (oldcat != null)
            {

                     oldcat.Name = newname;
                     context.SaveChanges();
                     return Ok("updated successfully");

            }

                else return StatusCode(600, "there is no category with this id in the data base ");
            }


    [HttpGet("{Name:alpha}")]
    public IActionResult searchforproduct(string Name)
    {

        var catfound = context.Categories.FirstOrDefault(x => x.Name == Name);
        if (catfound != null)
        {
            CategoryDto tobesent = new CategoryDto()
            {
                Name = Name,
            
                Id= catfound.Id
            };

            return Ok(tobesent);

        }
        else
        {
            return StatusCode(700, "cant find this category");

        }

        }

    }
}

