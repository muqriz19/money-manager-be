
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using moneyManagerBE.Class;
using moneyManagerBE.Logs;
using moneyManagerBE.Models;
using moneyManagerBE.Services.Records;
using moneyManagerBE.Services.Users;

namespace moneyManagerBE.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class LogsController : ControllerBase
    {
        private readonly IUsersService _usersServices;
        private readonly ILogsService _logsService;
        private readonly IRecordsService _recordsService;


        public LogsController(IUsersService usersService, ILogsService logsService, IRecordsService recordsService)
        {
            _usersServices = usersService;
            _logsService = logsService;
            _recordsService = recordsService;
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(Response<Log>), 200)]
        public IActionResult CreateLog([FromBody] LogDto logDto)
        {
            var userExistDbResponse = _usersServices.CheckUser(logDto.UserId);

            if (userExistDbResponse.IsSuccess == false)
            {
                var response = new Response<string[]>
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = userExistDbResponse.Message
                };

                return BadRequest(response);
            }

            // check if record exist
            bool recordExist = _recordsService.DoesExist(logDto.RecordId);

            if (recordExist == false)
            {
                var response = new Response<string[]>
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = "Record Does Not Exist"
                };

                return BadRequest(response);
            }

            DbResponse<LogResponseDto> dbResponse = _logsService.AddLog(logDto);

            if (dbResponse.IsSuccess)
            {
                var newReponse = new Response<LogResponseDto>
                {
                    Status = StatusCodes.Status200OK,
                    Message = dbResponse.Message,
                    Data = dbResponse.Data
                };

                return Ok(newReponse);
            }
            else
            {
                var newReponse = new Response<string[]>
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = dbResponse.Message,
                };

                return BadRequest(newReponse);
            }
        }

        [Authorize]
        [HttpPut]
        public IActionResult UpdateLog([FromBody] LogDto log)
        {
            var userExistDbResponse = _usersServices.CheckUser(log.UserId);

            if (userExistDbResponse.IsSuccess == false)
            {
                var response = new Response<Log>
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = userExistDbResponse.Message
                };

                return BadRequest(response);
            }

            // check if record exist
            bool recordExist = _recordsService.DoesExist(log.RecordId);

            if (recordExist == false)
            {
                var response = new Response<Log>
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = "Record Does Not Exist"
                };

                return BadRequest(response);
            }

            bool logExist = _logsService.DoesExistId(log.Id);

            if (logExist == false)
            {
                var response = new Response<Log>
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = "Log Does Not Exist"
                };

                return BadRequest(response);
            }

            DbResponse<LogResponseDto> dbResponse = _logsService.UpdateLog(log);

            if (dbResponse.IsSuccess)
            {
                var newReponse = new Response<LogResponseDto>
                {
                    Status = StatusCodes.Status200OK,
                    Message = dbResponse.Message,
                    Data = dbResponse.Data
                };

                return Ok(newReponse);
            }
            else
            {
                var newReponse = new Response<LogResponseDto>
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = dbResponse.Message,
                };

                return BadRequest(newReponse);
            }
        }

        [Authorize]
        [HttpDelete("{logId}")]
        public IActionResult DeleteLogById(int logId)
        {
            if (_logsService.DoesExistId(logId))
            {
                var logDeleteDbResponse = _logsService.DeleteLogById(logId);

                var response = new Response<Log>
                {
                    Status = StatusCodes.Status200OK,
                    Message = logDeleteDbResponse.Message
                };


                return Ok(response);
            }
            else
            {
                var response = new Response<Log>
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = "Log not found"
                };


                return BadRequest(response);
            }
        }
    }
}