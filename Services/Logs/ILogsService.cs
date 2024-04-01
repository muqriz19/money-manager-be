using moneyManagerBE.Class;
using moneyManagerBE.Models;

namespace moneyManagerBE.Logs
{
    public interface ILogsService
    {
        public DbResponse<Log> AddLog(Log log);

        public DbResponse<Log> UpdateLog(Log log);

        public bool DoesExist(Log log);
        public bool DoesExistId(int logId);

        public DbResponseList<List<Log>> GetAllLogs(int userId, int pageNumber, int pageSize, string search);

        public List<Log> GetLogsByRecordID(int recordId);

        public DbResponse<Log> DeleteLogById(int logId);
    }
}