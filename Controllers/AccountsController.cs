using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using moneyManagerBE.Class;
using moneyManagerBE.Models;
using moneyManagerBE.Services.Accounts;
using moneyManagerBE.Services.Users;

namespace moneyManagerBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountsService _accountsServices;
        private readonly IUsersService _usersServices;

        public AccountsController(
            IAccountsService accountsServices,
            IUsersService usersServices
            )
        {
            _accountsServices = accountsServices;
            _usersServices = usersServices;
        }

        [Authorize]
        [HttpGet("{userId}")]
        public IActionResult GetAllAccounts(int userId, [FromQuery] PaginationFilter filter)
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

            var dbResponseList = _accountsServices.GetAllAccounts(userId, validFilter.PageNumber, validFilter.PageSize, validFilter?.Search);

            var response = new ResponseList<List<Account>>
            {
                Status = StatusCodes.Status200OK,
                Message = "Retrieved data",
                Data = dbResponseList.Data,
                Total = dbResponseList.Total
            };

            return Ok(response);
        }

        [Authorize]
        [HttpGet("{userId}/{accountId}")]
        public IActionResult GetAccountById(int userId, int accountId)
        {
            var dbResponse = _accountsServices.GetAccountById(userId, accountId);

            if (!dbResponse.IsSuccess)
            {
                return NotFound(new Response<Account>
                {
                    Message = dbResponse.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }

            return Ok(new Response<Account>
            {
                Data = dbResponse.Data,
                Message = dbResponse.Message,
                Status = StatusCodes.Status200OK
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateAccount([FromBody] Account account)
        {
            var userExistDbResponse = _usersServices.CheckUser(account.UserId);

            if (!userExistDbResponse.IsSuccess)
            {
                return BadRequest(new Response<Account>
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = userExistDbResponse.Message
                });
            }

            DbResponse<Account> dbResponse = _accountsServices.AddAccount(account);

            if (!dbResponse.IsSuccess)
            {
                var failedResponse = new Response<string[]>
                {
                    Status = StatusCodes.Status409Conflict,
                    Message = dbResponse.Message,
                };

                return Conflict(failedResponse);
            }

            var response = new Response<Account>
            {
                Status = StatusCodes.Status201Created,
                Message = dbResponse.Message,
                Data = dbResponse.Data
            };

            return Ok(response);
        }

        [Authorize]
        [HttpPut]
        public IActionResult UpdateAccount([FromBody] Account account)
        {
            var userExistDbResponse = _usersServices.CheckUser(account.UserId);

            if (!userExistDbResponse.IsSuccess)
            {
                var failedResponse = new Response<Account>
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = userExistDbResponse.Message
                };

                return BadRequest(failedResponse);
            }

            if (account.Id == 0)
            {
                return BadRequest(new Response<string[]>
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = "This account does not exist, failed to update"
                });
            }

            DbResponse<Account> dbResponse = _accountsServices.UpdateAccount(account);

            var response = new Response<Account>
            {
                Status = StatusCodes.Status200OK,
                Message = dbResponse.Message,
                Data = dbResponse.Data
            };

            return Ok(response);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteAccount(int id)
        {
            DbResponse<List<string>> dbResponse = _accountsServices.DeleteAccount(id);

            if (!dbResponse.IsSuccess)
            {
                var failedResponse = new Response<string[]>
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = dbResponse.Message,
                };

                return BadRequest(failedResponse);
            }

            var response = new Response<string[]>
            {
                Status = StatusCodes.Status200OK,
                Message = dbResponse.Message,
            };

            return Ok(response);

        }
    }
}