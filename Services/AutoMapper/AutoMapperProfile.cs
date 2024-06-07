using AutoMapper;
using moneyManagerBE.Dtos;
using moneyManagerBE.Models;
using moneyManagerBE.Services.Categories;
using moneyManagerBE.Utils;
using Newtonsoft.Json;

namespace moneyManagerBE.Services.AutoMapper
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile(
            )
        {
            CreateMap<User, LoginResponseDto>();

            CreateMap<User, UserDto>();

            CreateMap<Record, RecordResponseDto>();

            CreateMap<Record, RecordResponseDto>()
            .ForMember(dest => dest.Logs, opt => opt.MapFrom(src => LogToLogResponseDto(src.Logs)));

            CreateMap<Log, LogResponseDto>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => CategoryJsonStringToCategory(src.Category)))
            .AfterMap<LogAction>();

            CreateMap<LogDto, Log>()
            .AfterMap<CategoryAction>();

            CreateMap<TransactionDto, Transaction>()
            .AfterMap<TransactionAction>();

            CreateMap<TransactionDto, TransactionResponseDto>()
            .AfterMap<TransactionDtoToTransactionResponseAction>();

            CreateMap<Transaction, TransactionResponseDto>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => CategoryJsonStringToCategory(src.Category)));
        }

        public Category CategoryJsonStringToCategory(string category)
        {
            Category categoryObject = JsonConvert.DeserializeObject<Category>(category);

            return categoryObject;
        }

        public T JsonStringToType<T>(string jsonString)
        {
            T categoryObject = JsonConvert.DeserializeObject<T>(jsonString);

            return categoryObject;
        }

        public int CategoryJsonStringToCategoryId(string category)
        {
            int categoryId = JsonConvert.DeserializeObject<Category>(category).Id;

            return categoryId;
        }

        public List<LogResponseDto> LogToLogResponseDto(List<Log> logs)
        {
            List<LogResponseDto> allLogResponseDtos = [];

            foreach (Log theLog in logs)
            {
                LogResponseDto theLogResponse = new LogResponseDto
                {
                    Category = CategoryJsonStringToCategory(theLog.Category),
                    CategoryId = CategoryJsonStringToCategoryId(theLog.Category),
                    CreatedDate = theLog.CreatedDate,
                    Description = theLog.Description,
                    Id = theLog.Id,
                    Name = theLog.Name,
                    RecordId = theLog.RecordId,
                    UserId = theLog.UserId,
                    Value = theLog.Value,
                    Transactions = TransctionsToTransactionResponseDtos(theLog.Transactions)
                };

                allLogResponseDtos.Add(theLogResponse);
            }

            return allLogResponseDtos;
        }

        public List<TransactionResponseDto> TransctionsToTransactionResponseDtos(List<Transaction> transactions)
        {
            List<TransactionResponseDto> allList = [];

            foreach (Transaction transaction in transactions)
            {
                TransactionResponseDto newTransaction = new TransactionResponseDto
                {
                    Category = JsonStringToType<Category>(transaction.Category),
                    CreatedDate = transaction.CreatedDate,
                    Description = transaction.Description,
                    Id = transaction.Id,
                    LogId = transaction.LogId,
                    Name = transaction.Name,
                    TransactionType = transaction.TransactionType,
                    UserId = transaction.UserId,
                    Value = transaction.Value
                };

                allList.Add(newTransaction);
            }

            return allList;
        }
    }
}