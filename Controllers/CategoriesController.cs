using Microsoft.AspNetCore.Mvc;
using moneyManagerBE.Class;
using moneyManagerBE.Models;
using moneyManagerBE.Services.Categories;

namespace moneyManagerBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesServices _categoriesServices;
        public CategoriesController(ICategoriesServices categoriesServices)
        {
            _categoriesServices = categoriesServices;
        }

        [HttpPost]
        public IActionResult CreateCategory([FromBody] Category category)
        {
            DbResponse<Category> dbResponse = _categoriesServices.AddCategory(category);

            if (dbResponse.IsSuccess)
            {
                var response = new Response<Category>
                {
                    Status = StatusCodes.Status200OK,
                    Message = dbResponse.Message,
                    Data = dbResponse.Data
                };

                return Ok(response);
            }
            else
            {
                var response = new Response<string[]>
                {
                    Status = StatusCodes.Status409Conflict,
                    Message = dbResponse.Message,
                };

                return Conflict(response);
            }
        }

        [HttpGet]
        public IActionResult GetAllCategories([FromQuery] PaginationFilter filter)
        {
            PaginationFilter validFilter;

            if (filter.Search is not null)
            {
                validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.Search);
            }
            else
            {
                validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            }

            DbResponseList<List<Category>> dbResponseList = _categoriesServices.GetAllCategories(validFilter.PageNumber, validFilter.PageSize, validFilter?.Search);

            var response = new ResponseList<List<Category>>
            {
                Status = StatusCodes.Status200OK,
                Message = "Retrieved data",
                Data = dbResponseList.Data,
                Total = dbResponseList.Total
            };

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAccount(int id)
        {
            DbResponse<List<string>> dbResponse = _categoriesServices.DeleteCategory(id);

            if (dbResponse.IsSuccess)
            {
                var response = new Response<string[]>
                {
                    Status = StatusCodes.Status200OK,
                    Message = dbResponse.Message,
                    Data = new string[] { }
                };

                return Ok(response);
            }
            else
            {
                var response = new Response<string[]>
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = dbResponse.Message,
                    Data = new string[] { }
                };

                return BadRequest(response);
            }

        }

        [HttpPut]
        public IActionResult UpdateCategory([FromBody] Category category)
        {

            if (category.Id == 0)
            {
                var response = new Response<string[]>
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = "This category does not exist, failed to update"
                };

                return BadRequest(response);
            }
            else
            {
                DbResponse<Category> dbResponse = _categoriesServices.UpdateCategory(category);

                var response = new Response<Category>
                {
                    Status = StatusCodes.Status200OK,
                    Message = dbResponse.Message,
                    Data = dbResponse.Data
                };

                return Ok(response);
            }
        }
    }
}