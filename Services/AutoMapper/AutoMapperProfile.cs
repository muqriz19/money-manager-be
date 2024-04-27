using AutoMapper;
using moneyManagerBE.Dtos;
using moneyManagerBE.Models;
using Newtonsoft.Json;

namespace moneyManagerBE.Services.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>();

            CreateMap<Record, RecordResponseDto>();

            CreateMap<Record, RecordResponseDto>()
            .ForMember(dest => dest.Logs, opt => opt.MapFrom(src => LogToLogResponseDto(src.Logs)));

            CreateMap<Log, LogResponseDto>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => CategoryJsonStringToCategory(src.Category)));
            // .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => CategoryJsonStringToCategoryId(src.Category)));
        }

        public Category CategoryJsonStringToCategory(string category)
        {
            Console.WriteLine(category);
            Category categoryObject = JsonConvert.DeserializeObject<Category>(category);

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
                    Transactions = theLog.Transactions
                };

                allLogResponseDtos.Add(theLogResponse);
            }

            return allLogResponseDtos;
        }
    }
}