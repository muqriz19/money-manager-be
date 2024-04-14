using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using moneyManagerBE.Class;
using moneyManagerBE.Logs;
using moneyManagerBE.Models;
using moneyManagerBE.Transactions;

namespace moneyManagerBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionsService _transactionService;
        private readonly ILogsService _logsService;

        public TransactionsController(ITransactionsService transactionService, ILogsService logsService)
        {
            _transactionService = transactionService;
            _logsService = logsService;
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateTransaction([FromBody] Transaction transaction)
        {
            bool doesExist = _transactionService.DoesExistId(transaction.Id);

            if (doesExist)
            {
                return BadRequest(new Response<Transaction>
                {
                    Message = $"Transaction with {transaction.Id} already exists",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            bool logExist = _logsService.DoesExistId(transaction.LogId);

            if (logExist == false)
            {
                return BadRequest(new Response<Transaction>
                {
                    Message = $"Log with {transaction.LogId} does not exist",
                    Status = StatusCodes.Status400BadRequest
                });
            }


            DbResponse<Transaction> dbResponse = _transactionService.AddTransaction(transaction);

            return Ok(new Response<Transaction>
            {
                Status = StatusCodes.Status200OK,
                Message = dbResponse.Message,
                Data = dbResponse.Data
            });
        }

        [Authorize]
        [HttpDelete("{transactionId}")]
        public IActionResult DeleteTransactionById(int transactionId)
        {
            bool doesExist = _transactionService.DoesExistId(transactionId);

            if (doesExist == false)
            {
                return BadRequest(new Response<Transaction>
                {
                    Message = $"Transaction does not exists",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            DbResponse<Transaction> dbResponse = _transactionService.DeleteTransactionById(transactionId);

            return Ok(new Response<Transaction>
            {
                Status = StatusCodes.Status200OK,
                Message = dbResponse.Message,
            });
        }

        [Authorize]
        [HttpPut]
        public IActionResult UpdateTransaction([FromBody] Transaction transaction)
        {
            bool doesExist = _transactionService.DoesExistId(transaction.Id);

            if (doesExist == false)
            {
                return BadRequest(new Response<Transaction>
                {
                    Message = $"Transaction with {transaction.Id} does not exists",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            bool logExist = _logsService.DoesExistId(transaction.LogId);

            if (logExist == false)
            {
                return BadRequest(new Response<Transaction>
                {
                    Message = $"Log with {transaction.LogId} does not exist",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            DbResponse<Transaction> dbResponse = _transactionService.UpdateTransaction(transaction);

            return Ok(new Response<Transaction>
            {
                Status = StatusCodes.Status200OK,
                Message = dbResponse.Message,
                Data = dbResponse.Data
            });
        }
    }
}