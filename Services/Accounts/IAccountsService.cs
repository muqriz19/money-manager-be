using moneyManagerBE.Class;
using moneyManagerBE.Models;

namespace moneyManagerBE.Services.Accounts
{
    public interface IAccountsService
    {
        DbResponse<Account> AddAccount(Account account);
        DbResponse<Account> UpdateAccount(Account account);

        DbResponseList<List<Account>> GetAllAccounts(int pageNumber, int pageSize, string search);

        DbResponse<List<string>> DeleteAccount(int id);
    }
}