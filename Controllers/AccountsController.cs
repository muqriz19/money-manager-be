using Microsoft.AspNetCore.Mvc;
using moneyManagerBE.Class;
using moneyManagerBE.Models;
using moneyManagerBE.Services.Accounts;

namespace moneyManagerBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountsService _accountsService;
        public AccountsController(IAccountsService accountsService)
        {
            _accountsService = accountsService;
        }

        [HttpGet]
        public IActionResult GetAllAccounts([FromQuery] PaginationFilter filter)
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

            var dbResponseList = _accountsService.GetAllAccounts(validFilter.PageNumber, validFilter.PageSize, validFilter?.Search);

            var response = new ResponseList<List<Account>>
            {
                Status = StatusCodes.Status200OK,
                Message = "Retrieved data",
                Data = dbResponseList.Data,
                Total = dbResponseList.Total
            };

            return Ok(response);
        }

        [HttpPost]
        public IActionResult CreateAccount([FromBody] Account account)
        {
            if (ModelState.IsValid)
            {
                DbResponse<Account> dbResponse = _accountsService.AddAccount(account);

                if (dbResponse.IsSuccess)
                {
                    var response = new Response<Account>
                    {
                        Status = StatusCodes.Status201Created,
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
                        Data = new string[] { }
                    };

                    return Conflict(response);
                }
            }
            else
            {
                var response = new Response<string[]>
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = ModelState.Values.SelectMany(v => v.Errors).ToString()!,
                    Data = new string[] { }
                };

                return BadRequest(response);
            }
        }

        [HttpPut]
        public IActionResult UpdateAccount([FromBody] Account account)
        {
            if (ModelState.IsValid)
            {
                if (account.Id == 0)
                {
                    var response = new Response<string[]>
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Message = "This account does not exist, failed to update"
                    };

                    return BadRequest(response);
                }
                else
                {
                    DbResponse<Account> dbResponse = _accountsService.UpdateAccount(account);

                    var response = new Response<Account>
                    {
                        Status = StatusCodes.Status200OK,
                        Message = dbResponse.Message,
                        Data = dbResponse.Data
                    };

                    return Ok(response);
                }
            }
            else
            {
                var response = new Response<string[]>
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = ModelState.Values.SelectMany(v => v.Errors).ToString()!,
                    Data = new string[] { }
                };

                return BadRequest(response);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAccount(int id)
        {
            DbResponse<List<string>> dbResponse = _accountsService.DeleteAccount(id);

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
    }
}