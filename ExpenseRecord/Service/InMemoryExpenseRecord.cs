using ExpenseRecord.Models;

namespace ExpenseRecord.Service
{
    public class InMemoryExpenseRecord : IExpenseRecordService
    {
        private static readonly List<ExpenseRecordDto> _expenseRecordDtos = new();

        public async Task<ExpenseRecordDto> CreateAsync(ExpenseRecordDto newrecord)
        {
            _expenseRecordDtos.Add(newrecord);
            return GenerateTodoItemDto(newrecord);
        }

        public async Task<List<ExpenseRecordDto>> GetAllAsync()
        {
            return _expenseRecordDtos.Select(GenerateTodoItemDto).OrderByDescending(item => item.Date).ToList();
        }

        public async Task<ExpenseRecordDto?> GetAsync(string id)
        {
            var toDoItem = _expenseRecordDtos.Find(x => x.Id == id);
            if (toDoItem == null)
            {
                return null;
            }
            return GenerateTodoItemDto(toDoItem);

        }


        public Task<bool> RemoveAsync(string id)
        {
            var itemToBeRemoved = _expenseRecordDtos.Find(x => x.Id == id);
            if (itemToBeRemoved is null)
            {
                return Task.FromResult(false);
            }
            _expenseRecordDtos.Remove(itemToBeRemoved);
            return Task.FromResult(true);
        }


        public async Task<ExpenseRecordDto> ReplaceAsync(string id, ExpenseRecordDto updatedRecord)
        {
            var index = _expenseRecordDtos.FindIndex(x => x.Id == id);
            if (index >= 0)
            {
                updatedRecord.Date = _expenseRecordDtos[index].Date;
                _expenseRecordDtos[index] = updatedRecord;
            }
            return GenerateTodoItemDto(updatedRecord);
        }
        private ExpenseRecordDto GenerateTodoItemDto(ExpenseRecordDto expenseRecord)
        {
            return new ExpenseRecordDto
            {
                Description = expenseRecord.Description,
                Type = expenseRecord.Type,
                Amount = expenseRecord.Amount,
                Date = expenseRecord.Date,
                Id = expenseRecord.Id,
            };
        }
    }
}
