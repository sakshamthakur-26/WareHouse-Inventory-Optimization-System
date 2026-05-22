using Microsoft.EntityFrameworkCore;
using WareHouse_Optimization_System.DTOs.Transaction;
using WareHouse_Optimization_System.Models;

namespace WareHouse_Optimization_System.Services
{
    public class TransactionService
    {
        private readonly TransactionContext _context;

        public TransactionService(TransactionContext context)
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
                    Timestamp = t.Timestamp
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
                Timestamp = t.Timestamp
            };
        }

        public async Task<TransactionLogResponse> CreateTransactionAsync(CreateTransactionRequest request)
        {
            var transaction = new Transaction
            {
                ItemId = request.ItemId,
                Quantity = request.Quantity,
                Type = request.Type,
                Timestamp = DateTime.Now
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return new TransactionLogResponse
            {
                TransactionId = transaction.TransactionId,
                ItemId = transaction.ItemId,
                Quantity = transaction.Quantity,
                Type = transaction.Type,
                Timestamp = transaction.Timestamp
            };
        }
    }
}
