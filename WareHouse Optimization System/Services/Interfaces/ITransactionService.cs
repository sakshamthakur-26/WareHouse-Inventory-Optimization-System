using WareHouse_Optimization_System.DTOs.Transaction;

namespace WareHouse_Optimization_System.Services.Interfaces

{

    public interface ITransactionService
    {

        Task<ServiceResult<TransactionLogResponse>> CreateTransactionAsync(CreateTransactionRequest request);

        Task<ServiceResult<IEnumerable<TransactionLogResponse>>> GetAllTransactionsAsync();

        Task<ServiceResult<TransactionLogResponse>> GetTransactionByIdAsync(int id);

        Task<ServiceResult<IEnumerable<TransactionLogResponse>>> GetTransactionsByVendorId(int vendorId);

        Task<ServiceResult<IEnumerable<TransactionLogResponse>>> GetTransactionsByDateRangeAsync(DateTime start, DateTime end);

        Task<ServiceResult<IEnumerable<TransactionLogResponse>>> GetItemHistoryAsync(int itemId);

        Task<ServiceResult<IEnumerable<TransactionLogResponse>>> GetTransactionsByTypeAsync(string type);

    }

}