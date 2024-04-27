using Microsoft.EntityFrameworkCore;
using moneyManagerBE.Class;
using moneyManagerBE.Data;
using moneyManagerBE.Models;

namespace moneyManagerBE.Services.Records
{
    public class RecordsService : IRecordsService
    {
        private readonly AppDbContext _appDbContext;

        public RecordsService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public DbResponse<Record> AddRecord(Record record)
        {
            // check if same
            var sameRecordFound = _appDbContext.Records.Where(theRecord => theRecord.Name == record.Name).FirstOrDefault();

            if (sameRecordFound != null)
            {
                return new DbResponse<Record>
                {
                    IsSuccess = false,
                    Message = "Same record already exists"
                };
            }
            else
            {
                _appDbContext.Records.Add(record);
                _appDbContext.SaveChanges();

                return new DbResponse<Record>
                {
                    IsSuccess = true,
                    Message = "Successfully created a record"
                };
            }
        }

        public DbResponseList<List<Record>> AllRecords(int userId, int accountId, int pageNumber, int pageSize, string search)
        {
            string searchTerm = search.ToLower();

            List<Record> allRecords;
            // if search use searched total, if not then db all count
            int totalCount = 0;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                allRecords = _appDbContext.Records
                .Where(theRecord => theRecord.UserId == userId)
                .Where(theRecord => theRecord.AccountId == accountId)
                .Where(theRecord =>
                theRecord.Name.ToLower().Contains(searchTerm) ||
                (theRecord.Description != null && theRecord.Description.ToLower().Contains(searchTerm))
                )
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

                totalCount = allRecords.Count();
            }
            else
            {
                allRecords = _appDbContext.Records
                .Where(theRecord => theRecord.UserId == userId)
                .Where(theRecord => theRecord.AccountId == accountId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

                totalCount = _appDbContext.Records.Count();
            }

            DbResponseList<List<Record>> dbResponseList = new DbResponseList<List<Record>>
            {
                Data = allRecords,
                IsSuccess = true,
                Total = totalCount,
                Message = "Success getting records"
            };

            return dbResponseList;
        }

        public bool DoesExist(int recordId)
        {
            var record = _appDbContext.Records.FirstOrDefault(theRecord => theRecord.Id == recordId);

            return record != null ? true : false;
        }

        public DbResponse<Record> GetRecordById(int userId, int accountId, int recordId)
        {
            var foundData = _appDbContext.Records
            .Where(data => data.Id == recordId)
            .Include(e => e.Logs)
            .ThenInclude(e => e.Transactions)
            .FirstOrDefault();

            if (foundData != null)
            {
                return new DbResponse<Record>
                {
                    IsSuccess = true,
                    Message = $"Record found",
                    Data = foundData
                };
            }
            else
            {
                return new DbResponse<Record>
                {
                    IsSuccess = false,
                    Message = $"Record of {recordId} does not exist"
                };
            }
        }

        public DbResponse<List<string>> RemoveRecord(int id)
        {
            var record = _appDbContext.Records.FirstOrDefault(data => data.Id == id);

            if (record != null)
            {
                _appDbContext.Records.Remove(record);
                _appDbContext.SaveChanges();

                return new DbResponse<List<string>>()
                {
                    IsSuccess = true,
                    Message = "Deleted record of " + id
                };
            }
            else
            {
                return new DbResponse<List<string>>()
                {
                    IsSuccess = false,
                    Message = $"Record of {id} does not exist"
                };
            }
        }

        public DbResponse<Record> UpdateRecord(Record record)
        {
            _appDbContext.Records.Update(record);
            _appDbContext.SaveChanges();

            return new DbResponse<Record>
            {
                IsSuccess = true,
                Message = "Record updated successful",
                Data = record
            };
        }
    }
}