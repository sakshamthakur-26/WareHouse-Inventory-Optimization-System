using Microsoft.EntityFrameworkCore;
using WareHouse_Optimization_System.Db;
using WareHouse_Optimization_System.DTOs.Transaction;
using WareHouse_Optimization_System.Models;
using WareHouse_Optimization_System.Services.Interfaces;

namespace WareHouse_Optimization_System.Services.Implementations
{
    public class TransactionService : ITransactionService
    {
        private readonly WarehouseDbContext _context;

        public TransactionService(WarehouseDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<IEnumerable<TransactionLogResponse>>> GetAllTransactionsAsync()
        {
            try
            {
                var transactions = await _context.Transactions
                    .Select(t => new TransactionLogResponse
                    {
                        TransactionId = t.TransactionId,
                        ItemId = t.ItemId,
                        Quantity = t.Quantity,
                        Type = t.Type,
                        Timestamp = t.Timestamp,
                        VendorId = t.VendorId
                    })
                    .ToListAsync();

                if (!transactions.Any())
                    return ServiceResult<IEnumerable<TransactionLogResponse>>
                        .Failure("No transactions found.");

                return ServiceResult<IEnumerable<TransactionLogResponse>>
                    .Success(transactions);
            }
            catch (Exception ex)
            {
                return ServiceResult<IEnumerable<TransactionLogResponse>>
                    .Failure($"Error fetching transactions: {ex.Message}");
            }
        }

        public async Task<ServiceResult<TransactionLogResponse>> GetTransactionByIdAsync(int id)
        {
            try
            {
                var t = await _context.Transactions.FindAsync(id);

                if (t == null)
                    return ServiceResult<TransactionLogResponse>.Failure("Transaction not found.");

                var response = new TransactionLogResponse
                {
                    TransactionId = t.TransactionId,
                    ItemId = t.ItemId,
                    Quantity = t.Quantity,
                    Type = t.Type,
                    Timestamp = t.Timestamp,
                    VendorId = t.VendorId
                };

                return ServiceResult<TransactionLogResponse>.Success(response);
            }
            catch (Exception ex)
            {
                return ServiceResult<TransactionLogResponse>
                    .Failure($"Error fetching transaction: {ex.Message}");
            }
        }

        public async Task<ServiceResult<IEnumerable<TransactionLogResponse>>> GetTransactionsByVendorId(int vendorId)
        {
            try
            {
                var transactions = await _context.Transactions
                    .Where(t => t.VendorId == vendorId)
                    .Select(t => new TransactionLogResponse
                    {
                        TransactionId = t.TransactionId,
                        ItemId = t.ItemId,
                        Quantity = t.Quantity,
                        Type = t.Type,
                        Timestamp = t.Timestamp,
                        VendorId = t.VendorId
                    }).ToListAsync();

                if (!transactions.Any())
                    return ServiceResult<IEnumerable<TransactionLogResponse>>
                        .Failure("No transactions found for this vendor.");

                return ServiceResult<IEnumerable<TransactionLogResponse>>
                    .Success(transactions);
            }
            catch (Exception ex)
            {
                return ServiceResult<IEnumerable<TransactionLogResponse>>
                    .Failure($"Error fetching vendor transactions: {ex.Message}");
            }
        }

        public async Task<ServiceResult<IEnumerable<TransactionLogResponse>>> GetTransactionsByDateRangeAsync(DateTime start, DateTime end)
        {
            try
            {
                var transactions = await _context.Transactions
                    .Where(t => t.Timestamp >= start && t.Timestamp <= end)
                    .Select(t => new TransactionLogResponse
                    {
                        TransactionId = t.TransactionId,
                        ItemId = t.ItemId,
                        Quantity = t.Quantity,
                        Type = t.Type,
                        Timestamp = t.Timestamp,
                        VendorId = t.VendorId
                    }).ToListAsync();

                if (!transactions.Any())
                    return ServiceResult<IEnumerable<TransactionLogResponse>>
                        .Failure("No transactions found in this date range.");

                return ServiceResult<IEnumerable<TransactionLogResponse>>
                    .Success(transactions);
            }
            catch (Exception ex)
            {
                return ServiceResult<IEnumerable<TransactionLogResponse>>
                    .Failure($"Error fetching date-range transactions: {ex.Message}");
            }
        }

        public async Task<ServiceResult<IEnumerable<TransactionLogResponse>>> GetItemHistoryAsync(int itemId)
        {
            try
            {
                var transactions = await _context.Transactions
                    .Where(t => t.ItemId == itemId)
                    .OrderBy(t => t.Timestamp)
                    .Select(t => new TransactionLogResponse
                    {
                        TransactionId = t.TransactionId,
                        ItemId = t.ItemId,
                        Quantity = t.Quantity,
                        Type = t.Type,
                        Timestamp = t.Timestamp,
                        VendorId = t.VendorId
                    }).ToListAsync();

                if (!transactions.Any())
                    return ServiceResult<IEnumerable<TransactionLogResponse>>
                        .Failure("No history found for this item.");

                return ServiceResult<IEnumerable<TransactionLogResponse>>
                    .Success(transactions);
            }
            catch (Exception ex)
            {
                return ServiceResult<IEnumerable<TransactionLogResponse>>
                    .Failure($"Error fetching item history: {ex.Message}");
            }
        }

        public async Task<ServiceResult<IEnumerable<TransactionLogResponse>>> GetTransactionsByTypeAsync(string type)
        {
            try
            {
                var transactions = await _context.Transactions
                    .Where(t => t.Type == type)
                    .Select(t => new TransactionLogResponse
                    {
                        TransactionId = t.TransactionId,
                        ItemId = t.ItemId,
                        Quantity = t.Quantity,
                        Type = t.Type,
                        Timestamp = t.Timestamp,
                        VendorId = t.VendorId
                    }).ToListAsync();

                if (!transactions.Any())
                    return ServiceResult<IEnumerable<TransactionLogResponse>>
                        .Failure($"No transactions found of type {type}");

                return ServiceResult<IEnumerable<TransactionLogResponse>>
                    .Success(transactions);
            }
            catch (Exception ex)
            {
                return ServiceResult<IEnumerable<TransactionLogResponse>>
                    .Failure($"Error fetching type transactions: {ex.Message}");
            }
        }

        public async Task<ServiceResult<TransactionLogResponse>> CreateTransactionAsync(CreateTransactionRequest request)
        {
            try
            {
                if (request == null)
                    return ServiceResult<TransactionLogResponse>.Failure("Invalid request.");

                var stock = await _context.StockItems
                    .FirstOrDefaultAsync(s => s.ItemId == request.ItemId);

                if (stock == null)
                    return ServiceResult<TransactionLogResponse>.Failure("Stock item not found.");

                var transaction = new Transaction
                {
                    ItemId = request.ItemId,
                    Quantity = request.Quantity,
                    Type = request.Type,
                    Timestamp = DateTime.Now,
                    VendorId = request.VendorId
                };

                _context.Transactions.Add(transaction);
                var result = await _context.SaveChangesAsync();

                if (result <= 0)
                    return ServiceResult<TransactionLogResponse>.Failure("Failed to create transaction.");

                var response = new TransactionLogResponse
                {
                    TransactionId = transaction.TransactionId,
                    ItemId = transaction.ItemId,
                    Quantity = transaction.Quantity,
                    Type = transaction.Type,
                    Timestamp = transaction.Timestamp,
                    VendorId = transaction.VendorId
                };

                return ServiceResult<TransactionLogResponse>.Success(response);
            }
            catch (DbUpdateException ex)
            {
                return ServiceResult<TransactionLogResponse>
                    .Failure($"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return ServiceResult<TransactionLogResponse>
                    .Failure($"Unexpected error: {ex.Message}");
            }
        }
    }
}