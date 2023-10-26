using ExpenseRecord.Models;

namespace ExpenseRecord.Service
{
    public interface IExpenseRecordService
    {
        Task<ExpenseRecordDto> CreateAsync(ExpenseRecordDto expenseRecordDto);
        Task<List<ExpenseRecordDto>> GetAllAsync();
        Task<ExpenseRecordDto?> GetAsync(string id);
        Task<bool> RemoveAsync(string id);
        // Task<ExpenseRecordDto> ReplaceAsync(string id, ExpenseRecordDto expenseRecordDto);
    }
}
