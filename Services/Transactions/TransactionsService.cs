using Microsoft.EntityFrameworkCore;
using moneyManagerBE.Class;
using moneyManagerBE.Data;
using moneyManagerBE.Models;

namespace moneyManagerBE.Transactions
{
    public class TransactionsService : ITransactionsService
    {
        private readonly AppDbContext _appDbContext;

        public TransactionsService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public DbResponse<Transaction> AddTransaction(Transaction transaction)
        {
            _appDbContext.Transactions.Add(transaction);
            _appDbContext.SaveChanges();

            return new DbResponse<Transaction>
            {
                Data = transaction,
                IsSuccess = true,
                Message = "Transaction successfully created"
            };
        }

        public DbResponse<Transaction> DeleteTransactionById(int transactionId)
        {
            var transaction = _appDbContext.Transactions.Where(entity => entity.Id == transactionId).FirstOrDefault();
            _appDbContext.Transactions.Remove(transaction);
            _appDbContext.SaveChanges();

            return new DbResponse<Transaction>
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

        public DbResponse<Transaction> UpdateTransaction(Transaction transaction)
        {
            _appDbContext.Transactions.Update(transaction);
            _appDbContext.SaveChanges();

            return new DbResponse<Transaction>
            {
                Data = transaction,
                IsSuccess = true,
                Message = "Transaction successfully updated"
            };
        }
    }
}