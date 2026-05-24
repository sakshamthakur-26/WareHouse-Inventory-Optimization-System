using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WareHouse_Optimization_System.Models;
using WareHouse_Optimization_System.Services;
using WareHouse_Optimization_System.DTOs.Transaction;

namespace WareHouse_Optimization_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly TransactionService _service;

        public TransactionsController(TransactionService service)
        {
            _service = service; 
        }

        // GET: api/Transactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionLogResponse>>> GetTransactions()
        {
            try
            {
                var transactions = await _service.GetAllTransactionsAsync();
                return Ok(transactions); // 200 OK
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching transactions.");
            }
        }

        // GET: api/Transactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionLogResponse>> GetTransactionByIdAsync(int id)
        {
            try
            {
                var transaction = await _service.GetTransactionByIdAsync(id);

                if (transaction == null)
                {
                    return NotFound("Transaction not found."); // 404 Not Found
                }

                return Ok(transaction); // 200 OK
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the transaction.");
            }
        }
        // POST: api/Transactions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [HttpPost]
        public async Task<ActionResult<TransactionLogResponse>> PostTransaction(CreateTransactionRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Invalid request data.");
                }

                var createdTransaction = await _service.CreateTransactionAsync(request);

                return CreatedAtAction(
                    nameof(GetTransactionByIdAsync),
                    new { id = createdTransaction.Data.TransactionId },
                    createdTransaction
                ); // 201 Created
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred while creating the transaction.");
            }
        }
    }
    
}
