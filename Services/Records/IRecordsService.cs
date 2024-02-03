using moneyManagerBE.Class;
using moneyManagerBE.Models;

namespace moneyManagerBE.Services.Records
{
    public interface IRecordsService
    {
        DbResponse<Record> AddRecord(Record record);
        DbResponseList<List<Record>> AllRecords(int userId, int accountId, int pageNumber, int pageSize, string search);

        DbResponse<List<string>> RemoveRecord(int id);

        DbResponse<Record> GetRecordById(int userId, int accountId, int recordId);

        DbResponse<Record> UpdateRecord(Record record);
    }
}