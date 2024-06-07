using moneyManagerBE.Class;
using moneyManagerBE.Models;

namespace moneyManagerBE.Logs
{
    public interface ILogsService
    {
        public DbResponse<LogResponseDto> AddLog(LogDto log);

        public DbResponse<LogResponseDto> UpdateLog(LogDto log);

        public bool DoesExist(LogDto log);
        public bool DoesExistId(int logId);

        public DbResponse<LogResponseDto> DeleteLogById(int logId);
    }
}