using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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
    public class RecordsController : ControllerBase
    {

        private readonly IRecordsService _recordsService;
        private readonly IUsersService _usersService;

        private readonly ILogsService _logsService;

        public RecordsController(IRecordsService recordsService, IUsersService usersService, ILogsService logsService)
        {
            _recordsService = recordsService;
            _usersService = usersService;
            _logsService = logsService;
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateRecords([FromBody] Record record)
        {
            // check user
            var checkUserDbResponse = _usersService.CheckUser(record.UserId);

            if (checkUserDbResponse.IsSuccess == false)
            {
                var response = new Response<Record>
                {
                    Message = checkUserDbResponse.Message,
                    Status = 400
                };

                return BadRequest(response);
            }

            // check record account must not be null or 0
            if (record.AccountId == 0)
            {
                var response = new Response<Record>
                {
                    Message = "Account Id must not be null/zero value",
                    Status = 400
                };

                return BadRequest(response);
            }

            var dbResponse = _recordsService.AddRecord(record);

            if (dbResponse.IsSuccess)
            {
                var myResponse = new Response<Record>
                {
                    Data = dbResponse.Data,
                    Message = dbResponse.Message,
                    Status = 200
                };

                return Ok(myResponse);
            }
            else
            {
                var badResponse = new Response<Record>
                {
                    Status = 403,
                    Message = dbResponse.Message
                };

                return BadRequest(badResponse);
            }

        }

        [Authorize]
        [HttpGet("{userId}/{accountId}")]
        public IActionResult GetAllRecords(int userId, int accountId, [FromQuery] PaginationFilter filter)
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

            DbResponseList<List<Record>> dbResponseList = _recordsService.AllRecords(userId, accountId, validFilter.PageNumber, validFilter.PageSize, validFilter?.Search);

            var response = new ResponseList<List<Record>>
            {
                Status = StatusCodes.Status200OK,
                Message = "Retrieved data",
                Data = dbResponseList.Data,
                Total = dbResponseList.Total
            };

            return Ok(response);
        }

        [Authorize]
        [HttpGet("{userId}/{accountId}/{recordId}")]
        public IActionResult GetRecordById(int userId, int accountId, int recordId)
        {
            var dbResponse = _recordsService.GetRecordById(userId, accountId, recordId);

            if (dbResponse.IsSuccess)
            {
                dbResponse.Data.Logs = _logsService.GetLogsByRecordID(recordId);

                return Ok(new Response<Record>
                {
                    Data = dbResponse.Data,
                    Message = dbResponse.Message,
                    Status = 200
                });
            }
            else
            {
                return NotFound(new Response<Record>
                {
                    Message = dbResponse.Message,
                    Status = 404
                });
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteRecord(int id)
        {
            var dbResponse = _recordsService.RemoveRecord(id);

            if (dbResponse.IsSuccess)
            {
                var response = new Response<List<string>>
                {
                    Message = dbResponse.Message,
                    Status = 200
                };

                return Ok(response);
            }
            else
            {
                var response = new Response<List<string>>
                {
                    Message = dbResponse.Message,
                    Status = 4001
                };

                return BadRequest(response);
            }
        }

        [Authorize]
        [HttpPut]
        public IActionResult UpdateRecord([FromBody] Record record)
        {
            var userExistDbResponse = _usersService.CheckUser(record.UserId);

            if (userExistDbResponse.IsSuccess == false)
            {
                var response = new Response<Account>
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = userExistDbResponse.Message
                };

                return BadRequest(response);
            }

            if (record.Id == 0)
            {
                var response = new Response<string[]>
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = "This record does not exist, failed to update"
                };

                return BadRequest(response);
            }
            else
            {
                DbResponse<Record> dbResponse = _recordsService.UpdateRecord(record);

                var response = new Response<Record>
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