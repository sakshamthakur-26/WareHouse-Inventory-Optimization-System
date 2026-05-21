using System.Numerics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Transaction_log.Model
{
    public class TransactionContext : DbContext
    {
        public TransactionContext(DbContextOptions<TransactionContext> options) : base(options) { }
        public DbSet<Transaction> Transactions { get; set; }
        
    }
}
