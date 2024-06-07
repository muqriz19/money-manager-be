using moneyManagerBE.Class;
using moneyManagerBE.Models;

namespace moneyManagerBE.Transactions
{
    public interface ITransactionsService
    {
        public DbResponse<TransactionResponseDto> AddTransaction(TransactionDto transaction);

        public bool DoesExistId(int transactionId);

        public DbResponse<TransactionResponseDto> UpdateTransaction(TransactionDto transaction);

        public DbResponse<TransactionResponseDto> DeleteTransactionById(int transactionId);

        public DbResponse<List<TransactionResponseDto>> GetTransactionsByLogId(int logId);
    }
}