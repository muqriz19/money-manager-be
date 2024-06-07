using AutoMapper;
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
        private readonly ICategoriesService _categoriesService;
        private readonly IMapper _mapper;


        public LogsService(AppDbContext appDbContext, ICategoriesService categoriesServices, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _categoriesService = categoriesServices;
            _mapper = mapper;
        }

        public DbResponse<LogResponseDto> AddLog(LogDto logDto)
        {
            if (DoesExist(logDto))
            {
                return new DbResponse<LogResponseDto>
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

                var logReponseDto = _mapper.Map<LogResponseDto>(newLog);

                return new DbResponse<LogResponseDto>
                {
                    Data = logReponseDto,
                    IsSuccess = true,
                    Message = "Log successfully created"
                };
            }
        }

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

        public DbResponse<LogResponseDto> DeleteLogById(int logId)
        {
            var log = _appDbContext.Logs.FirstOrDefault(theLog => theLog.Id == logId);

            if (log != null)
            {
                _appDbContext.Logs.Remove(log);
                _appDbContext.SaveChanges();

                return new DbResponse<LogResponseDto>
                {
                    IsSuccess = true,
                    Message = "Deleted account of " + logId
                };
            }
            else
            {
                return new DbResponse<LogResponseDto>
                {
                    IsSuccess = false,
                    Message = $"Log of {logId} does not exist"
                };
            }
        }

        public DbResponse<LogResponseDto> UpdateLog(LogDto log)
        {
            var logDb = _mapper.Map<Log>(log);

            _appDbContext.Logs.Update(logDb);
            _appDbContext.SaveChanges();

            var logResponse = _mapper.Map<LogResponseDto>(logDb);

            return new DbResponse<LogResponseDto>
            {
                IsSuccess = true,
                Message = "Log successfully updated",
                Data = logResponse
            };
        }
    }
}