using moneyManagerBE.Class;
using moneyManagerBE.Models;

namespace moneyManagerBE.Logs {
    public interface ILogsService {
        public DbResponse<Log> AddLog(Log log);
    }
}