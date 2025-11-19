using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using personal_accountant.DTOs;
using personal_accountant.Services.Interfaces;
using personal_accountant.Utilities;

namespace personal_accountant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationServiceInterface _service;

        public AuthenticationController(IAuthenticationServiceInterface service)
        {
            _service = service;
        }
        [HttpPost("login")]
        public async Task<ActionResult<LoggedUserDTO>> Login(LoggingDTO request)
        {
            Result<LoggedUserDTO> result = await _service.LogInAsync(request);

            if (result.Success)
                return Ok(result.Data);
            return StatusCode(result.ErrorCode, result.Message);
        }
        [HttpPost("forget-password")]
        public async Task<ActionResult> ForgetPassword([FromBody] string email)
        {
            Result<bool> result = await _service.ForgetPasswordAsync(email);

            if (result.Success)
                return Ok();
            return StatusCode(result.ErrorCode, result.Message);
        }
        [HttpPost("reset-password")]
        public async Task<ActionResult<LoggedUserDTO>> ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            Result<bool> result = await _service.ResetPasswordAsync(resetPasswordDTO);

            if (result.Success)
                return Ok(result.Data);
            return StatusCode(result.ErrorCode, result.Message);
        }

     }
}
