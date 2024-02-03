using Microsoft.AspNetCore.Mvc;
using moneyManagerBE.Class;
using moneyManagerBE.Dtos;
using moneyManagerBE.Models;
using moneyManagerBE.Services.Authorization;

namespace moneyManagerBE.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthorization _authorizationService;

        public AuthorizationController(IAuthorization authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [Route("Register")]
        [HttpPost]
        public IActionResult RegisterUser([FromBody] User user)
        {
            DbResponse<UserDto> dbResponse = _authorizationService.AddUser(user);

            if (dbResponse.IsSuccess)
            {
                var response = new Response<UserDto>
                {
                    Status = StatusCodes.Status201Created,
                    Message = dbResponse.Message,
                    Data = dbResponse.Data
                };

                return Ok(response);
            }
            else
            {
                var response = new Response<UserDto>
                {
                    Status = StatusCodes.Status409Conflict,
                    Message = dbResponse.Message,
                };

                return Conflict(response);
            }
        }

        [Route("Login")]
        [HttpPost]
        public IActionResult Login([FromBody] Login login)
        {
            DbResponse<LoginData> dbResponse = _authorizationService.Login(login.Email, login.Password);

            if (dbResponse.IsSuccess)
            {
                dbResponse.Data.Token = _authorizationService.GenerateToken();

                var response = new Response<LoginData>
                {
                    Status = StatusCodes.Status201Created,
                    Message = dbResponse.Message,
                    Data = dbResponse.Data
                };

                return Ok(response);
            }
            else
            {
                var response = new Response<LoginData>
                {
                    Status = StatusCodes.Status403Forbidden,
                    Message = dbResponse.Message,
                };

                return new ObjectResult(response) { StatusCode = 403 };
            }
        }

        [Route("ForgotPassword")]
        [HttpPost]
        public IActionResult ForgotPassword([FromBody] ForgotPassword forgotPassword)
        {
            bool userExist = _authorizationService.CheckEmail(forgotPassword.Email);

            if (userExist)
            {
                var response = new Response<string[]>
                {
                    Status = StatusCodes.Status200OK,
                    Message = "Initialising...",
                };

                _authorizationService.ForgotPassword(forgotPassword.Email);

                return Ok(response);
            }
            else
            {
                var response = new Response<string[]>
                {
                    Status = StatusCodes.Status404NotFound,
                    Message = "User does not exist, are you sure this email is correct?",
                };

                return NotFound(response);
            }
        }

        [Route("ResetPassword")]
        [HttpPost]
        public IActionResult ResetPassword([FromBody] ResetPassword resetPassword)
        {
            bool got = _authorizationService.CheckEmail(resetPassword.Email);

            if (got)
            {
                var dbResponse = _authorizationService.ChangePassword(resetPassword.Email, resetPassword.newPassword);

                var response = new Response<string[]>
                {
                    Status = StatusCodes.Status200OK,
                    Message = dbResponse.Message,
                };

                return Ok(response);

            }
            else
            {
                var response = new Response<string[]>
                {
                    Status = StatusCodes.Status404NotFound,
                    Message = "User does not exist, are you sure this email is correct?",
                };

                return NotFound(response);
            }
        }
    }
}