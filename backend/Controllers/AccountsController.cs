using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using personal_accountant.DTOs;
using personal_accountant.DTOs.Mapper;
using personal_accountant.Services.Interfaces;
using personal_accountant.Utilities;
using System.Security.Claims;
using System.Security.Principal;

namespace personal_accountant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountServiceInterface _service;

        public AccountsController(IAccountServiceInterface service)
        {
            _service = service;
        }

        [HttpGet]
   
        public async Task<ActionResult<IEnumerable<AccountDetailDTO>>> GetAccounts([FromQuery] int? userId)
        {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            if (!User.IsInRole("Admin"))
            {
                userId = currentUserId;
            }
            DateTime time = DateTime.Now;
            Result<IEnumerable<AccountDetailDTO>> result = await _service.GetAllAsync(userId);
            if (result.Success)
                return Ok(result.Data);
            return StatusCode(result.ErrorCode, result.Message);
        }


        [HttpGet("{id:int}", Name = "GetAccountById")]
        public async Task<ActionResult<AccountDTO>> GetAccount(int id)
        {
            Result<AccountDTO> result = await _service.FindAsync(id);
            if (result.Success)
                return Ok(result.Data);
            return StatusCode(result.ErrorCode, result.Message);
        }
        [HttpPost]
        public async Task<ActionResult<AccountDTO>> AddAccount(AccountDTO newAccount)
        {

            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            if (!User.IsInRole("Admin"))
            {
                newAccount.UserId = currentUserId;
            }

            Result<int> result = await _service.AddNewAsync(newAccount);
            if (result.Success)
            {
                newAccount.Id = result.Data;
                return CreatedAtRoute("GetAccountById", new { id = result.Data }, newAccount);
            }

            return StatusCode(result.ErrorCode, result.Message);
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult<AccountDTO>> UpdateAccount(int id, AccountDTO updatedAccount)
        {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            if (!User.IsInRole("Admin"))
            {
                updatedAccount.UserId = currentUserId;
            }

            Result<bool> result = await _service.UpdateAsync(id, updatedAccount);
            if (result.Success)
                return Ok(updatedAccount);
            return StatusCode(result.ErrorCode, result.Message);
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteAccount(int id)
        {
            Result<bool> result = await _service.DeleteAsync(id);
            if (result.Success)
                return Ok();
            return StatusCode(result.ErrorCode, result.Message);
        }
    }
}
