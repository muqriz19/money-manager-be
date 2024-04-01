using Microsoft.EntityFrameworkCore;
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
            if (DoesExist(log))
            {
                return new DbResponse<Log>
                {
                    Data = log,
                    IsSuccess = false,
                    Message = "Log with the same name already created"
                };
            }
            else
            {
                _appDbContext.Logs.Add(log);
                _appDbContext.SaveChanges();

                return new DbResponse<Log>
                {
                    Data = log,
                    IsSuccess = true,
                    Message = "Log successfully created"
                };
            }
        }

        public bool DoesExist(Log log)
        {
            var logFind = _appDbContext.Logs.Where(dataLog => dataLog.UserId == log.UserId).Where(dataLog => dataLog.Name == log.Name).FirstOrDefault();

            return logFind != null ? true : false;
        }

        public bool DoesExistId(int logId)
        {
            var logFind = _appDbContext.Logs.AsNoTracking().FirstOrDefault(dataLog => dataLog.Id == logId);

            return logFind != null ? true : false;
        }

        public DbResponseList<List<Log>> GetAllLogs(int userId, int pageNumber, int pageSize, string search)
        {
            string searchTerm = search.ToLower();

            List<Log> allLogs = [];
            // if search use searched total, if not then db all count
            int totalCount = 0;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                allLogs = _appDbContext.Logs
                .Where(data => data.UserId == userId)
                .Where(data =>
                data.Name.ToLower().Contains(searchTerm) ||
                (data.Description != null && data.Description.ToLower().Contains(searchTerm))
                )
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

                totalCount = allLogs.Count();
            }
            else
            {
                allLogs = _appDbContext.Logs
                .Where(data => data.UserId == userId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

                totalCount = _appDbContext.Logs.Count();
            }

            DbResponseList<List<Log>> dbResponseList = new DbResponseList<List<Log>>
            {
                Data = allLogs,
                IsSuccess = true,
                Total = totalCount,
                Message = "Success getting logs"
            };

            return dbResponseList;
        }

        public List<Log> GetLogsByRecordID(int recordId)
        {
            var allLogs = _appDbContext.Logs.Where(log => log.RecordId == recordId).ToList();

            return allLogs;
        }

        public DbResponse<Log> DeleteLogById(int logId)
        {
            var log = _appDbContext.Logs.FirstOrDefault(theLog => theLog.Id == logId);

            if (log != null)
            {
                _appDbContext.Logs.Remove(log);
                _appDbContext.SaveChanges();

                return new DbResponse<Log>
                {
                    IsSuccess = true,
                    Message = "Deleted account of " + logId
                };
            }
            else
            {
                return new DbResponse<Log>
                {
                    IsSuccess = false,
                    Message = $"Log of {logId} does not exist"
                };
            }
        }

        public DbResponse<Log> UpdateLog(Log log)
        {
            _appDbContext.Logs.Update(log);
            _appDbContext.SaveChanges();

            return new DbResponse<Log>
            {
                IsSuccess = true,
                Message = "Log successfully updated",
                Data = log
            };
        }
    }
}