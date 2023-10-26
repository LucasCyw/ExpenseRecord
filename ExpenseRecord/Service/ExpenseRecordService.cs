using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ExpenseRecord.Models;
using ExpenseRecord.Service;
using System.Net;
using Amazon.Runtime.Internal;

namespace ExpenseRecord.Services
{
    public class ExpenseRecordService : IExpenseRecordService
    {
        private readonly IMongoCollection<ExpenseRecordDto> _ExpenseRecordCollection;

        public ExpenseRecordService(IOptions<ExpenseRecordDatabaseSettings> expenseRecordDatabaseSettings)
        {
            var mongoClient = new MongoClient(expenseRecordDatabaseSettings.Value.ConnectionString);
            var mongoDB = mongoClient.GetDatabase(expenseRecordDatabaseSettings.Value.DatabaseName);
            _ExpenseRecordCollection = mongoDB.GetCollection<ExpenseRecordDto>(expenseRecordDatabaseSettings.Value.CollectionName);
        }


        public async Task<ExpenseRecordDto> CreateAsync(ExpenseRecordDto request)
        {
            var record = new ExpenseRecordDto()
            {
                Id = Guid.NewGuid().ToString(),
                Description = request.Description,
                Type = request.Type,
                Amount = request.Amount,
                Date = request.Date ?? DateTime.Now,
            };
            await _ExpenseRecordCollection.InsertOneAsync(record);
            return GenerateTodoItemDto(record);
        }

        public async Task<List<ExpenseRecordDto>> GetAllAsync()
        {
            var records = await _ExpenseRecordCollection.Find(_ => true).ToListAsync();
            if (records != null && records.Count > 1)
            {
                records = records.OrderByDescending(item => item.Date).ToList();
            }
            // return new Response<List<ExpenseRecordDto>>(HttpStatusCode.OK, items);
            return records.Select(GenerateTodoItemDto).ToList();
        }

        public async Task<ExpenseRecordDto?> GetAsync(string id)
        {
            var todoItem = await _ExpenseRecordCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            return todoItem == null ? null : GenerateTodoItemDto(todoItem);
        }

        public async Task<bool> RemoveAsync(string id)
        {
            var result = await _ExpenseRecordCollection.DeleteOneAsync(x => x.Id == id);
            return result.DeletedCount > 0;
        }

        public async Task<ExpenseRecordDto> ReplaceAsync(string id, ExpenseRecordDto updatedToDoItem)
        {
            await _ExpenseRecordCollection.ReplaceOneAsync(x => x.Id == id, updatedToDoItem);
            return GenerateTodoItemDto(updatedToDoItem);
        }

        private ExpenseRecordDto GenerateTodoItemDto(ExpenseRecordDto record)
        {
            return new ExpenseRecordDto
            {
                Description = record.Description,
                Type = record.Type,
                Amount = record.Amount,
                Date = record.Date,
                Id = record.Id,
            };
        }
    }
}
