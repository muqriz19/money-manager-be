using Microsoft.AspNetCore.Mvc;
using moneyManagerBE.Class;
using moneyManagerBE.Dtos;
using moneyManagerBE.Models;
using moneyManagerBE.Services.Authorization;
using moneyManagerBE.Services.Hasher;
using moneyManagerBE.Services.Users;

namespace moneyManagerBE.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthorization _authorizationService;
        private readonly IUsersService _usersService;

        private readonly IHasher _hasher;

        public AuthorizationController(
            IAuthorization authorizationService,
            IUsersService usersService,
            IHasher hasher
            )
        {
            _authorizationService = authorizationService;
            _usersService = usersService;
            _hasher = hasher;
        }

        [Route("Register")]
        [HttpPost]
        public IActionResult RegisterUser([FromBody] User user)
        {
            DbResponse<UserDto> dbResponse = _usersService.AddUser(user);

            if (!dbResponse.IsSuccess)
            {
                return Conflict(new Response<UserDto>
                {
                    Status = StatusCodes.Status409Conflict,
                    Message = dbResponse.Message,
                });
            }

            var response = new Response<UserDto>
            {
                Status = StatusCodes.Status201Created,
                Message = dbResponse.Message,
                Data = dbResponse.Data
            };

            return Ok(response);
        }

        [Route("Login")]
        [HttpPost]
        public IActionResult Login([FromBody] Login login)
        {
            DbResponse<LoginResponseDto> dbResponse = _authorizationService.Login(login.Email, login.Password);

            if (dbResponse.IsSuccess)
            {
                try
                {
                    var token = _authorizationService.GenerateToken();
                    dbResponse.Data!.Token = token;

                    var response = new Response<LoginResponseDto>
                    {
                        Status = StatusCodes.Status201Created,
                        Message = dbResponse.Message,
                        Data = dbResponse.Data
                    };

                    return Ok(response);
                }
                catch (NullReferenceException e)
                {
                    // Log the exception if necessary
                    // _logger.LogError(e, "A null reference occurred while generating the token.");
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while generating the token.");
                }
            }
            else
            {
                var response = new Response<LoginResponseDto>
                {
                    Status = StatusCodes.Status403Forbidden,
                    Message = dbResponse.Message
                };

                return Unauthorized(response);
            }
        }

        [Route("ForgotPassword")]
        [HttpPost]
        public IActionResult ForgotPassword([FromBody] ForgotPassword forgotPassword)
        {
            bool doesUserExist = _usersService.CheckEmail(forgotPassword.Email);

            if (!doesUserExist)
            {
                var userNotFoundResponse = new Response<string[]>
                {
                    Status = StatusCodes.Status404NotFound,
                    Message = "User does not exist, are you sure this email is correct?"
                };

                return NotFound(userNotFoundResponse);
            }

            var operationResponse = _authorizationService.ForgotPasswordOperation(forgotPassword.Email);

            if (!operationResponse.IsSuccess)
            {
                var userNotFoundResponse = new Response<string[]>
                {
                    Status = StatusCodes.Status404NotFound,
                    Message = "User does not exist, are you sure this email is correct?"
                };

                return NotFound(userNotFoundResponse);
            }

            var response = new Response<string>
            {
                Status = StatusCodes.Status200OK,
                Message = "Initialising...",
                Data = operationResponse.Data
            };

            return Ok(response);
        }

        [Route("ResetPassword")]
        [HttpPost]
        public IActionResult ResetPassword([FromBody] ResetPassword resetPassword)
        {
            bool doesEmailExist = _usersService.CheckEmail(resetPassword.Email);

            if (!doesEmailExist)
            {
                return NotFound(new Response<string[]>
                {
                    Status = StatusCodes.Status404NotFound,
                    Message = "User does not exist, are you sure this email is correct?"
                });
            }

            var dbResponseResetPassword = _authorizationService.CheckResetPasswordHashIsCorrect(resetPassword.Email, resetPassword.ResetPasswordTempHash);

            if (!dbResponseResetPassword.IsSuccess)
            {
                return BadRequest(new Response<string[]>
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = dbResponseResetPassword.Message
                });
            }

            var dbResponse = _authorizationService.ChangePassword(resetPassword.Email, resetPassword);

            var response = new Response<string[]>
            {
                Status = StatusCodes.Status200OK,
                Message = dbResponse.Message
            };

            return Ok(response);
        }
    }
}