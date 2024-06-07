using AutoMapper;
using moneyManagerBE.Logs;
using moneyManagerBE.Models;
using moneyManagerBE.Services.Categories;
using moneyManagerBE.Transactions;
using Newtonsoft.Json;

namespace moneyManagerBE.Utils
{
    public class CategoryAction : IMappingAction<LogDto, Log>
    {
        private readonly ICategoriesService _categoryServices;

        public CategoryAction(ICategoriesService categoriesServices)
        {
            _categoryServices = categoriesServices;
        }

        public void Process(LogDto source, Log destination, ResolutionContext context)
        {
            destination.Category = JsonConvert.SerializeObject(_categoryServices.GetCategoryById(source.CategoryId).Data);
        }
    }

    public class TransactionAction : IMappingAction<TransactionDto, Transaction>
    {
        private readonly ICategoriesService _categoryServices;

        public TransactionAction(ICategoriesService categoriesServices)
        {
            _categoryServices = categoriesServices;

        }

        public void Process(TransactionDto source, Transaction destination, ResolutionContext context)
        {
            destination.Category = JsonConvert.SerializeObject(_categoryServices.GetCategoryById(source.CategoryId).Data);
        }
    }

    public class TransactionDtoToTransactionResponseAction : IMappingAction<TransactionDto, TransactionResponseDto>
    {
        private readonly ICategoriesService _categoryServices;

        public TransactionDtoToTransactionResponseAction(ICategoriesService categoriesServices)
        {
            _categoryServices = categoriesServices;

        }

        public void Process(TransactionDto source, TransactionResponseDto destination, ResolutionContext context)
        {
            Console.WriteLine(source.CategoryId);
            destination.Category = _categoryServices.GetCategoryById(source.CategoryId).Data;
        }
    }

    public class LogAction : IMappingAction<Log, LogResponseDto>
    {
        private readonly ITransactionsService _transactionsService;

        public LogAction(ITransactionsService transactionsService)
        {
            _transactionsService = transactionsService;

        }
        public void Process(Log source, LogResponseDto destination, ResolutionContext context)
        {
            destination.Transactions = _transactionsService.GetTransactionsByLogId(source.Id).Data;
        }
    }
}