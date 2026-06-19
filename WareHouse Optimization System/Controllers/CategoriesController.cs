using Microsoft.AspNetCore.Mvc;
using WareHouse_Optimization_System.DTOs.Category;
using WareHouse_Optimization_System.Services;
using WareHouse_Optimization_System.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WareHouse_Optimization_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {

        private readonly ICategoryService _services;
        public CategoriesController(ICategoryService services)
        {
            _services = services;
        }
        // GET: api/<CategoriesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var categories = _services.GetAllCategoriesAsync().Result;
            if (!categories.IsSuccess)
            {
                return new string[] { categories.ErrorMessage };
            }
            return (IEnumerable<string>)categories;
        }

        // GET api/<CategoriesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CategoriesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateCategoryDto categorydto)
        {
            if (categorydto == null)
            {
                return BadRequest("Category data is required.");
            }

            var result = await _services.CreateCategoryAsync(categorydto);
            if (result.IsSuccess)
            {
                return Ok(true); 
            }

            return BadRequest(result.ErrorMessage);
        }
        

        // PUT api/<CategoriesController>/5
        [HttpPut("{id}")]
         public void Put(int id, [FromBody] string value)
        {

        }

        // DELETE api/<CategoriesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
