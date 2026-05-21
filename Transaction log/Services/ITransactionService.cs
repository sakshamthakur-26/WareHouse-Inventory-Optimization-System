using Transaction_log.DTOs;

namespace Transaction_log.Services
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionLogResponse>> GetAllTransactionsAsync();
        Task<TransactionLogResponse?> GetTransactionByIdAsync(int id);
        Task<TransactionLogResponse> CreateTransactionAsync(CreateTransactionRequest request);
    }
}
