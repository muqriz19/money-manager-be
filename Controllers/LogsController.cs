
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using moneyManagerBE.Class;
using moneyManagerBE.Models;
using moneyManagerBE.Services.Users;

namespace moneyManagerBE.Logs
{

    [ApiController]
    [Route("api/[controller]")]
    public class LogsController : ControllerBase
    {
        private readonly IUsersService _usersServices;

        public LogsController(IUsersService usersService)
        {
            _usersServices = usersService;
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateLog([FromBody] Log log)
        {
            Console.WriteLine(log.Name);
            var userExistDbResponse = _usersServices.CheckUser(log.UserId);

            if (userExistDbResponse.IsSuccess == false)
            {
                var response = new Response<Account>
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = userExistDbResponse.Message
                };

                return BadRequest(response);
            }

            

            return Ok();
        }
    }
}