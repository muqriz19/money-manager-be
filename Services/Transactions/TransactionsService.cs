using AutoMapper;
using Microsoft.EntityFrameworkCore;
using moneyManagerBE.Class;
using moneyManagerBE.Data;
using moneyManagerBE.Models;
using moneyManagerBE.Services.Categories;
using Newtonsoft.Json;

namespace moneyManagerBE.Transactions
{
    public class TransactionsService : ITransactionsService
    {
        private readonly AppDbContext _appDbContext;
        private readonly ICategoriesService _categoryServices;

        private readonly IMapper _mapper;

        public TransactionsService(AppDbContext appDbContext, ICategoriesService categoriesServices, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _categoryServices = categoriesServices;
            _mapper = mapper;
        }

        public DbResponse<TransactionResponseDto> AddTransaction(TransactionDto transaction)
        {
            Transaction newTransaction = new Transaction
            {
                Category = JsonConvert.SerializeObject(_categoryServices.GetCategoryById(transaction.CategoryId).Data),
                CreatedDate = transaction.CreatedDate,
                Description = transaction.Description,
                Id = transaction.Id,
                LogId = transaction.LogId,
                Name = transaction.Name,
                TransactionType = transaction.TransactionType,
                UserId = transaction.UserId,
                Value = transaction.Value,
            };

            _appDbContext.Transactions.Add(newTransaction);
            _appDbContext.SaveChanges();

            var transactionResponseDto = _mapper.Map<TransactionResponseDto>(newTransaction);

            return new DbResponse<TransactionResponseDto>
            {
                Data = transactionResponseDto,
                IsSuccess = true,
                Message = "Transaction successfully created"
            };
        }

        public DbResponse<TransactionResponseDto> DeleteTransactionById(int transactionId)
        {
            var transaction = _appDbContext.Transactions.Where(entity => entity.Id == transactionId).FirstOrDefault();
            _appDbContext.Transactions.Remove(transaction);
            _appDbContext.SaveChanges();

            var transactionResponseDto = _mapper.Map<TransactionResponseDto>(transaction);


            return new DbResponse<TransactionResponseDto>
            {
                IsSuccess = true,
                Message = "Transaction successfully deleted",
            };
        }

        public bool DoesExistId(int transactionId)
        {
            var transaction = _appDbContext.Transactions.AsNoTracking().FirstOrDefault(transaction => transaction.Id == transactionId);
            return transaction != null ? true : false;
        }

        public DbResponse<TransactionResponseDto> UpdateTransaction(TransactionDto transaction)
        {
            var transactionData = _mapper.Map<Transaction>(transaction);

            _appDbContext.Transactions.Update(transactionData);
            _appDbContext.SaveChanges();

            var transactionResponseDto = _mapper.Map<TransactionResponseDto>(transaction);

            return new DbResponse<TransactionResponseDto>
            {
                Data = transactionResponseDto,
                IsSuccess = true,
                Message = "Transaction successfully updated"
            };
        }

        public DbResponse<List<TransactionResponseDto>> GetTransactionsByLogId(int logId)
        {
            var transactions = _appDbContext.Transactions.Where(transaction => transaction.LogId == logId).ToArray();

            var transactionResponseDto = _mapper.Map<List<TransactionResponseDto>>(transactions);

            return new DbResponse<List<TransactionResponseDto>>
            {
                Data = transactionResponseDto,
                IsSuccess = true,
                Message = "Transaction successfully retrieved"
            };
        }
    }
}