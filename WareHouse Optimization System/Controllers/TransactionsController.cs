using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WareHouse_Optimization_System.DTOs.Transaction;
using WareHouse_Optimization_System.Services.Implementations;
using WareHouse_Optimization_System.Services.Interfaces;

namespace WareHouse_Optimization_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
      
        private readonly ITransactionService _service;

        public TransactionsController(ITransactionService service)
        {
            _service = service;
        }

        // GET: api/Transactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionLogResponse>>> GetTransactions()
        {
            var result = await _service.GetAllTransactionsAsync();
            if (!result.IsSuccess)
                return NotFound(result.ErrorMessage);

            return Ok(result.Data); // 200 OK
        }

        // GET: api/Transactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionLogResponse>> GetTransactionByIdAsync(int id)
        {
            var result = await _service.GetTransactionByIdAsync(id);

            if (!result.IsSuccess)
                return NotFound(result.ErrorMessage); // 404 Not Found

            return Ok(result.Data); // 200 OK
        }

        // POST: api/Transactions
        [HttpPost]
        public async Task<ActionResult<TransactionLogResponse>> PostTransaction(CreateTransactionRequest request)
        {
            if (request == null)
                return BadRequest("Invalid request data.");

            var createdTransaction = await _service.CreateTransactionAsync(request);

            if (!createdTransaction.IsSuccess)
                return BadRequest(createdTransaction.ErrorMessage);

            return CreatedAtAction(
                nameof(GetTransactionByIdAsync),
                new { id = createdTransaction.Data.TransactionId },
                createdTransaction.Data
            ); // 201 Created
        }

        // GET: api/Transactions/vendor?vendorId=5
        [HttpGet("vendor")]
        public async Task<ActionResult<IEnumerable<TransactionLogResponse>>> GetTransactionsByVendorIdAsync(int vendorId)
        {
            var result = await _service.GetTransactionsByVendorId(vendorId);

            if (!result.IsSuccess)
                return NotFound(result.ErrorMessage);

            return Ok(result.Data);
        }

        // GET: api/Transactions/date-range?start=2024-01-01&end=2024-12-31
        [HttpGet("date-range")]
        public async Task<ActionResult<IEnumerable<TransactionLogResponse>>> GetTransactionsByDateRange(DateTime start, DateTime end)
        {
            var result = await _service.GetTransactionsByDateRangeAsync(start, end);

            if (!result.IsSuccess)
                return NotFound(result.ErrorMessage);

            return Ok(result.Data);
        }

        // GET: api/Transactions/item?itemId=10
        [HttpGet("item")]
        public async Task<ActionResult<IEnumerable<TransactionLogResponse>>> GetItemHistory(int itemId)
        {
            var result = await _service.GetItemHistoryAsync(itemId);

            if (!result.IsSuccess)
                return NotFound(result.ErrorMessage);

            return Ok(result.Data);
        }

        // GET: api/Transactions/type?type=Sale
        [HttpGet("type")]
        public async Task<ActionResult<IEnumerable<TransactionLogResponse>>> GetTransactionsByType(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
                return BadRequest("Transaction type is required."); // 400

            var result = await _service.GetTransactionsByTypeAsync(type);

            if (!result.IsSuccess)
                return NotFound(result.ErrorMessage);

            return Ok(result.Data);
        }
    }
}
