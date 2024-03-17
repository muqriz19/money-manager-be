using moneyManagerBE.Class;
using moneyManagerBE.Data;
using moneyManagerBE.Logs;
using moneyManagerBE.Models;

namespace moneyManagerBE.Services.Logs
{
    public class LogsService : ILogsService
    {
        private readonly AppDbContext _appDbContext;

        public LogsService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public DbResponse<Log> AddLog(Log log)
        {
            throw new NotImplementedException();
        }
    }
}