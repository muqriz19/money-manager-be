using moneyManagerBE.Class;
using moneyManagerBE.Models;

namespace moneyManagerBE.Transactions
{
    public interface ITransactionsService
    {
        public DbResponse<Transaction> AddTransaction(Transaction transaction);

        public bool DoesExistId(int transactionId);

        public DbResponse<Transaction> UpdateTransaction(Transaction transaction);

        // public DbResponseList<List<Log>> GetAllLogs(int userId, int pageNumber, int pageSize, string search);

        public DbResponse<Transaction> DeleteTransactionById(int transactionId);
    }
}