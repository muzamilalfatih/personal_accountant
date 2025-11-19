using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using personal_accountant.DTOs;
using personal_accountant.Services.Interfaces;
using personal_accountant.Utilities;

namespace personal_accountant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionServiceInterface _service;

        public TransactionsController(ITransactionServiceInterface service)
        {
            _service = service;
        }

        [HttpGet]
 
        public async Task<ActionResult<IEnumerable<TransactionDetailDTO>>> GetTransactions([FromQuery] int? accountId)
        {
            Result<IEnumerable<TransactionDetailDTO>> result = await _service.GetAllAsync(accountId);
            if (result.Success)
                return Ok(result.Data);
            return StatusCode(result.ErrorCode, result.Message);
        }
        [HttpGet("{id:int}", Name = "GetTransactionById")]
        public async Task<ActionResult<TransactionDTO>> GetTransaction(int id)
        {
            Result<TransactionDTO> result = await _service.FindAsync(id);
            if (result.Success)
                return Ok(result.Data);
            return StatusCode(result.ErrorCode, result.Message);
        }
        [HttpPost]
        public async Task<ActionResult<TransactionDTO>> AddTransaction(TransactionDTO newTransaction)
        {
            Result<int> result = await _service.AddNewAsync(newTransaction);
            if (result.Success)
            {
                newTransaction.Id = result.Data;
                return CreatedAtRoute("GetTransactionById", new { id = result.Data }, new TransactionDTO(result.Data, newTransaction.Amount, DateTime.Now, newTransaction.Description, newTransaction.AccountId));
            }

            return StatusCode(result.ErrorCode, result.Message);
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult<TransactionDTO>> UpdateTransaction(int id, TransactionDTO updatedTransaction)
        {
            Result<bool> result = await _service.UpdateAsync(id, updatedTransaction);
            if (result.Success)
                return Ok(updatedTransaction);
            return StatusCode(result.ErrorCode, result.Message);
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteTransaction(int id)
        {
            Result<bool> result = await _service.DeleteAsync(id);
            if (result.Success)
                return Ok();
            return StatusCode(result.ErrorCode, result.Message);
        }
    }
}
