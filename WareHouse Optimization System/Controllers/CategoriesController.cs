using Microsoft.AspNetCore.Mvc;
using WareHouse_Optimization_System.DTOs.Category;
using WareHouse_Optimization_System.Services;
using WareHouse_Optimization_System.Services.Interfaces;



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


        [HttpGet]
        public async Task<ActionResult<List<string>>> Get()
        {
            var categories = await _services.GetAllCategoriesAsync();
            if (!categories.IsSuccess)
            {
                return BadRequest(categories.ErrorMessage);
            }

            return Ok(categories.Data);
        }


        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }


        //[HttpPost]
        //public async Task<IActionResult> Post([FromBody] CreateCategoryDto categorydto)
        //{
        //    if (categorydto == null)
        //    {
        //        return BadRequest("Category data is required.");
        //    }

        //    var result = await _services.CreateCategoryAsync(categorydto);
        //    if (result.IsSuccess)
        //    {
        //        return Ok(true); 
        //    }

        //    return BadRequest(result.ErrorMessage);
        //}


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateCategoryDto dto)
        {
            var result = await _services.CreateCategoryAsync(dto);
            return result.IsSuccess ? Ok(true) : BadRequest(result.ErrorMessage);
        }

        [HttpPost("assign")]
        public async Task<IActionResult> Assign([FromBody] AssignCategoryDto dto)
        {
            var result = await _services.AssignCategoryToZoneAsync(dto);
            return result.IsSuccess ? Ok(true) : BadRequest(result.ErrorMessage);
        }



    }
}
