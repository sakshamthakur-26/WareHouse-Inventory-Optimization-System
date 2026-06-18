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

        public async Task<IEnumerable<TransactionLogResponse>> GetAllTransactionsAsync()
        {
            return await _context.Transactions
                .Select(t => new TransactionLogResponse
                {
                    TransactionId = t.TransactionId,
                    ItemId = t.ItemId,
                    Quantity = t.Quantity,
                    Type = t.Type,
                    Timestamp = t.Timestamp ,
                    VendorId = t.VendorId
                })
                .ToListAsync();
        }

        public async Task<TransactionLogResponse?> GetTransactionByIdAsync(int id)
        {
            var t = await _context.Transactions.FindAsync(id);
            if (t == null) return null;

            return new TransactionLogResponse
            {
                TransactionId = t.TransactionId,
                ItemId = t.ItemId,
                Quantity = t.Quantity,
                Type = t.Type,
                Timestamp = t.Timestamp ,
                VendorId = t.VendorId
            };
        }

        public async Task<ServiceResult<TransactionLogResponse>> CreateTransactionAsync(CreateTransactionRequest request)
        {


            var stock = await _context.StockItems
                .FirstOrDefaultAsync(s => s.ItemId == request.ItemId);

            if (stock == null)
            {
                return ServiceResult<TransactionLogResponse>.Failure("Stock item not found");
            }

            var transaction = new Transaction
            {
                ItemId = request.ItemId,
                Quantity = request.Quantity,
                Type = request.Type,
                Timestamp = DateTime.Now,
                 VendorId = stock.VendorId
            };

            _context.Transactions.Add(transaction);
            var res = await _context.SaveChangesAsync();

            if (res <= 0)
            {
                return ServiceResult<TransactionLogResponse>.Failure("Failed to create transaction");
            }

            var responseLog = new TransactionLogResponse
            {
                TransactionId = transaction.TransactionId,
                ItemId = transaction.ItemId,
                Quantity = transaction.Quantity,
                Type = transaction.Type,
                Timestamp = transaction.Timestamp,
                VendorId = transaction.VendorId
            };
            return ServiceResult<TransactionLogResponse>.Success(responseLog);
        }
    }
}
