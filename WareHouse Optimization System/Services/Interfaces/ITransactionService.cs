using WareHouse_Optimization_System.DTOs.Transaction;

namespace WareHouse_Optimization_System.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<ServiceResult<TransactionLogResponse>> CreateTransactionAsync(CreateTransactionRequest request);
        Task<IEnumerable<TransactionLogResponse>> GetAllTransactionsAsync();
        Task<TransactionLogResponse?> GetTransactionByIdAsync(int id);
    }
}