using moneyManagerBE.Class;
using moneyManagerBE.Data;
using moneyManagerBE.Models;

namespace moneyManagerBE.Services.Accounts
{
    public class AccountsService : IAccountsService
    {
        private readonly AppDbContext _appdbContext;
        public AccountsService(AppDbContext appdbContext)
        {
            _appdbContext = appdbContext;
        }

        public DbResponse<Account> AddAccount(Account account)
        {
            var foundAccountWithSameName = _appdbContext.Accounts.Where(theAccount => theAccount.Name == account.Name).FirstOrDefault();

            if (foundAccountWithSameName != null)
            {
                return new DbResponse<Account>
                {
                    IsSuccess = false,
                    Message = "Account with the same name already exists, create another unique account"
                };

            }
            else
            {
                _appdbContext.Accounts.Add(account);
                _appdbContext.SaveChanges();

                return new DbResponse<Account>
                {
                    IsSuccess = true,
                    Message = "Created account successful",
                    Data = account
                };
            }
        }

        public DbResponseList<List<Account>> GetAllAccounts(int pageNumber, int pageSize, string search)
        {
            string searchTerm = search.ToLower();

            List<Account> allAccounts;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                allAccounts = _appdbContext.Accounts
                .Where(account =>
                account.Name.ToLower().Contains(searchTerm) ||
                account.Description.ToLower().Contains(searchTerm)
                )
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            }
            else
            {
                allAccounts = _appdbContext.Accounts
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            }


            DbResponseList<List<Account>> dbResponseList = new DbResponseList<List<Account>>
            {
                Data = allAccounts,
                IsSuccess = true,
                Total = _appdbContext.Accounts.Count(),
                Message = "Success getting accounts"
            };

            return dbResponseList;
        }
    }
}