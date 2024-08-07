using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using moneyManagerBE.Class;
using moneyManagerBE.Models;
using moneyManagerBE.Services.Categories;
using moneyManagerBE.Services.Users;

namespace moneyManagerBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesService _categoriesServices;
        private readonly IUsersService _usersServices;

        public CategoriesController(
            ICategoriesService categoriesServices,
            IUsersService usersServices
            )
        {
            _categoriesServices = categoriesServices;
            _usersServices = usersServices;
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateCategory([FromBody] Category category)
        {
            var userExistDbResponse = _usersServices.CheckUser(category.UserId);

            if (userExistDbResponse.IsSuccess == false)
            {
                return BadRequest(new Response<Account>
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = userExistDbResponse.Message
                });
            }

            DbResponse<Category> dbResponse = _categoriesServices.AddCategory(category);

            if (!dbResponse.IsSuccess)
            {
                return Conflict(new Response<string[]>
                {
                    Status = StatusCodes.Status409Conflict,
                    Message = dbResponse.Message,
                });
            }

            var response = new Response<Category>
            {
                Status = StatusCodes.Status200OK,
                Message = dbResponse.Message,
                Data = dbResponse.Data
            };

            return Ok(response);
        }

        [Authorize]
        [HttpGet("{userId}")]
        public IActionResult GetAllCategories(int userId, [FromQuery] PaginationFilter filter)
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

            DbResponseList<List<Category>> dbResponseList = _categoriesServices.GetAllCategories(userId, validFilter.PageNumber, validFilter.PageSize, validFilter?.Search);

            var response = new ResponseList<List<Category>>
            {
                Status = StatusCodes.Status200OK,
                Message = "Retrieved data",
                Data = dbResponseList.Data,
                Total = dbResponseList.Total
            };

            return Ok(response);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteAccount(int id)
        {
            DbResponse<List<string>> dbResponse = _categoriesServices.DeleteCategory(id);

            if (!dbResponse.IsSuccess)
            {
                return BadRequest(new Response<string[]>
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = dbResponse.Message,
                    Data = []
                });
            }

            var response = new Response<string[]>
            {
                Status = StatusCodes.Status200OK,
                Message = dbResponse.Message
            };

            return Ok(response);
        }

        [Authorize]
        [HttpPut]
        public IActionResult UpdateCategory([FromBody] Category category)
        {
            var userExistDbResponse = _usersServices.CheckUser(category.UserId);

            if (!userExistDbResponse.IsSuccess)
            {
                return BadRequest(new Response<Account>
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = userExistDbResponse.Message
                });
            }

            if (category.Id == 0)
            {
                return BadRequest(new Response<string[]>
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = "This category does not exist, failed to update"
                });
            }

            DbResponse<Category> dbResponse = _categoriesServices.UpdateCategory(category);

            var response = new Response<Category>
            {
                Status = StatusCodes.Status200OK,
                Message = dbResponse.Message,
                Data = dbResponse.Data
            };

            return Ok(response);
        }

        [Authorize]
        [HttpGet("/{categoryId}")]
        public IActionResult GetCategoryById(int categoryId)
        {
            var dbResponse = _categoriesServices.GetCategoryById(categoryId);

            if (!dbResponse.IsSuccess)
            {
                return NotFound(new Response<Category>
                {
                    Message = dbResponse.Message,
                    Status = 404
                });
            }

            return Ok(new Response<Category>
            {
                Data = dbResponse.Data,
                Message = dbResponse.Message,
                Status = 200
            });
        }
    }
}