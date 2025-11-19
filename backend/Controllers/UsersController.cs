using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Writers;
using personal_accountant.DTOs;
using personal_accountant.DTOs.Mapper;
using personal_accountant.Services;
using personal_accountant.Services.Interfaces;
using personal_accountant.Utilities;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace personal_accountant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserServiceInterface _service;

        public UsersController(IUserServiceInterface service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<IEnumerable<UserPublicDTO>>> GetUsers([FromQuery] int? currentUserId)
        {
            Result<IEnumerable<UserPublicDTO>> result = await _service.GetAllAsync(currentUserId);
            if (result.Success)
                return Ok(result.Data);
            return StatusCode(result.ErrorCode, result.Message);
        }
        [HttpGet("{id:int}", Name = "GetUserById")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            if (currentUserId != id && !User.IsInRole("Admin"))
            {
                return Forbid("Access Denied");
            }

            Result<UserDTO> result = await _service.FindAsync(id);
            if (result.Success)
                return Ok(result.Data.ToUserPublicDTO());
            return StatusCode(result.ErrorCode, result.Message);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<UserDTO>> AddUser(UserDTO newUser)
        {
            Result<int> result = await _service.AddNewAsync(newUser);
            if (result.Success)
            { 
                newUser.Id = result.Data;
                UserPublicDTO response = newUser.ToUserPublicDTO();
                return CreatedAtRoute("GetUserById", new { id =  result.Data }, response);
            }
                
            return StatusCode(result.ErrorCode, result.Message);
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult<UserDTO>> UpdateUser(int id, UserDTO updatedUser)
        {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            if (currentUserId != id && !User.IsInRole("Admin"))
            {
                return Forbid("Access Denied");
            }

            Result<bool> result = await _service.UpdateAsync(id, updatedUser);
            if (result.Success)
                return Ok(updatedUser);
            return StatusCode(result.ErrorCode, result.Message);
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            if (currentUserId != id && !User.IsInRole("Admin"))
            {
                return Forbid("Access Denied");
            }

            Result<bool> result = await _service.DeleteAsync(id);
            if (result.Success)
                return Ok();
            return StatusCode(result.ErrorCode, result.Message);
        }
        [HttpPatch("{id:int}")]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult> ToggleRole(int id , [FromBody] string role)
        {
            Result<bool> result = await _service.ToggleRoleAsync(id, role);
            if (result.Success)
                return Ok();
            return StatusCode(result.ErrorCode, result.Message);
        }
        [HttpPost("{id:int}/reset-password")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ChangePassword(int id, [FromBody] ChangePasswordDTO request)
        {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            if ( !User.IsInRole("Admin"))
            {
               id = currentUserId;
            }

            Result<bool> result = await _service.ChangedPassword(id, request);
            if (result.Success)
                return Ok();
            return StatusCode(result.ErrorCode, result.Message);
        }
        [AllowAnonymous]
        [HttpGet("confirm-email")]
        public async Task<ActionResult> ConfirmEmail([FromQuery] string token)
        {
            Result<bool> result = await _service.ConfirmEmail(token);

            if (result.Success)
                return Ok();
            return StatusCode(result.ErrorCode, result.Message);
        }
    }
}
