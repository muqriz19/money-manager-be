using Microsoft.EntityFrameworkCore;
using moneyManagerBE.Class;
using moneyManagerBE.Data;
using moneyManagerBE.Logs;
using moneyManagerBE.Models;
using moneyManagerBE.Services.Categories;
using Newtonsoft.Json;

namespace moneyManagerBE.Services.Logs
{
    public class LogsService : ILogsService
    {
        private readonly AppDbContext _appDbContext;
        private readonly ICategoriesServices _categoriesService;

        public LogsService(AppDbContext appDbContext, ICategoriesServices categoriesServices)
        {
            _appDbContext = appDbContext;
            _categoriesService = categoriesServices;
        }

        public DbResponse<Log> AddLog(LogDto logDto)
        {
            if (DoesExist(logDto))
            {
                return new DbResponse<Log>
                {
                    IsSuccess = false,
                    Message = "Log with the same name already created"
                };
            }
            else
            {
                Category category = _categoriesService.GetCategoryById(logDto.CategoryId).Data;

                Log newLog = new Log
                {
                    Id = logDto.Id,
                    Name = logDto.Name,
                    Description = logDto.Description,
                    RecordId = logDto.RecordId,
                    UserId = logDto.UserId,
                    Value = logDto.Value,
                    Category = JsonConvert.SerializeObject(category),
                    CreatedDate = logDto.CreatedDate
                };

                _appDbContext.Logs.Add(newLog);
                _appDbContext.SaveChanges();


                return new DbResponse<Log>
                {
                    Data = newLog,
                    IsSuccess = true,
                    Message = "Log successfully created"
                };
            }
        }

        // public DbResponse<Log> AddLog(LogDto logDto)
        // {
        //     if (DoesExist(logDto))
        //     {
        //         return new DbResponse<Log>
        //         {
        //             IsSuccess = false,
        //             Message = "Log with the same name already created"
        //         };
        //     }
        //     else
        //     {
        //         Category category = _categoriesService.GetCategoryById(logDto.CategoryId).Data;

        //         LogDb newLog = new LogDb
        //         {
        //             Id = logDto.Id,
        //             Name = logDto.Name,
        //             Description = logDto.Description,
        //             RecordId = logDto.RecordId,
        //             UserId = logDto.UserId,
        //             Value = logDto.Value,
        //             Category = JsonConvert.SerializeObject(category),
        //             CreatedDate = logDto.CreatedDate
        //         };

        //         _appDbContext.Logs.Add(newLog);
        //         _appDbContext.SaveChanges();

        //         Log finalLog = new Log
        //         {
        //             Id = logDto.Id,
        //             Name = logDto.Name,
        //             Description = logDto.Description,
        //             RecordId = logDto.RecordId,
        //             UserId = logDto.UserId,
        //             Value = logDto.Value,
        //             Category = category,
        //             CreatedDate = logDto.CreatedDate,
        //             Transactions = []
        //         };

        //         return new DbResponse<Log>
        //         {
        //             Data = finalLog,
        //             IsSuccess = true,
        //             Message = "Log successfully created"
        //         };
        //     }
        // }

        public bool DoesExist(LogDto log)
        {
            // does it exist within the context of this user
            var logFind = _appDbContext.Logs.Where(dataLog => dataLog.UserId == log.UserId).Where(dataLog => dataLog.Name == log.Name).FirstOrDefault();

            return logFind != null ? true : false;
        }

        public bool DoesExistId(int logId)
        {
            var logFind = _appDbContext.Logs.AsNoTracking().FirstOrDefault(dataLog => dataLog.Id == logId);

            return logFind != null ? true : false;
        }

        // public DbResponseList<List<Log>> GetAllLogs(int userId, int pageNumber, int pageSize, string search)
        // {
        //     string searchTerm = search.ToLower();

        //     List<Log> allLogs = [];
        //     List<LogDb> allLogsDb = [];

        //     // if search use searched total, if not then db all count
        //     int totalCount = 0;

        //     if (!string.IsNullOrEmpty(searchTerm))
        //     {
        //         allLogsDb = _appDbContext.Logs
        //         .Where(data => data.UserId == userId)
        //         .Where(data =>
        //         data.Name.ToLower().Contains(searchTerm) ||
        //         (data.Description != null && data.Description.ToLower().Contains(searchTerm))
        //         )
        //         .Skip((pageNumber - 1) * pageSize)
        //         .Take(pageSize)
        //         .ToList();

        //         totalCount = allLogs.Count();
        //     }
        //     else
        //     {
        //         allLogsDb = _appDbContext.Logs
        //         .Where(data => data.UserId == userId)
        //         .Skip((pageNumber - 1) * pageSize)
        //         .Take(pageSize)
        //         .ToList();

        //         totalCount = _appDbContext.Logs.Count();
        //     }

        //     foreach (LogDb log in allLogsDb)
        //     {
        //         Log newLog = new Log
        //         {
        //             Id = log.Id,
        //             CreatedDate = log.CreatedDate,
        //             Description = log.Description,
        //             Name = log.Name,
        //             RecordId = log.RecordId,
        //             Transactions = [],
        //             Category = JsonConvert.DeserializeObject<Category>(log.Category)
        //         };

        //         allLogs.Add(
        //             newLog
        //         );
        //     }

        //     DbResponseList<List<Log>> dbResponseList = new DbResponseList<List<Log>>
        //     {
        //         Data = allLogs,
        //         IsSuccess = true,
        //         Total = totalCount,
        //         Message = "Success getting logs"
        //     };

        //     return dbResponseList;
        // }

        // public List<Log> GetLogsByRecordID(int recordId)
        // {
        //     var allLogs = _appDbContext.Logs.Where(log => log.RecordId == recordId).ToList();

        //     return allLogs;
        // }

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
            // _appDbContext.Logs.Update(log);
            // _appDbContext.SaveChanges();

            return new DbResponse<Log>
            {
                IsSuccess = true,
                Message = "Log successfully updated",
                Data = log
            };
        }
    }
}