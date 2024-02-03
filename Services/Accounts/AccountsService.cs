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
            var foundAccountWithSameName = _appdbContext.Accounts
            .Where(theAccount => theAccount.UserId == account.UserId)
            .Where(theAccount => theAccount.Name == account.Name).FirstOrDefault();

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

        public DbResponse<Account> UpdateAccount(Account account)
        {
            _appdbContext.Accounts.Update(account);
            _appdbContext.SaveChanges();

            return new DbResponse<Account>
            {
                IsSuccess = true,
                Message = "Update account successful",
                Data = account
            };
        }

        public DbResponseList<List<Account>> GetAllAccounts(int userId, int pageNumber, int pageSize, string search)
        {
            string searchTerm = search.ToLower();

            List<Account> allAccounts;
            // if search use searched total, if not then db all count
            int totalCount = 0;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                allAccounts = _appdbContext.Accounts
                .Where(theAccount => theAccount.UserId == userId)
                .Where(account =>
                account.Name.ToLower().Contains(searchTerm) ||
                (account.Description != null && account.Description.ToLower().Contains(searchTerm))
                )
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

                totalCount = allAccounts.Count();
            }
            else
            {
                allAccounts = _appdbContext.Accounts
                .Where(theAccount => theAccount.UserId == userId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

                totalCount = _appdbContext.Accounts.Count();
            }


            DbResponseList<List<Account>> dbResponseList = new DbResponseList<List<Account>>
            {
                Data = allAccounts,
                IsSuccess = true,
                Total = totalCount,
                Message = "Success getting accounts"
            };

            return dbResponseList;
        }

        public DbResponse<List<string>> DeleteAccount(int id)
        {
            var account = _appdbContext.Accounts.FirstOrDefault(account => account.Id == id);

            if (account != null)
            {
                _appdbContext.Accounts.Remove(account);
                _appdbContext.SaveChanges();

                return new DbResponse<List<string>>()
                {
                    IsSuccess = true,
                    Message = "Deleted account of " + id
                };
            }
            else
            {
                return new DbResponse<List<string>>()
                {
                    IsSuccess = false,
                    Message = $"Account of {id} does not exist"
                };
            }
        }
    }
}