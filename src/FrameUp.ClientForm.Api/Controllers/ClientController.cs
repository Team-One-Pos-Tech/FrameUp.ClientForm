using FrameUp.ClientForm.Application.Contracts;
using FrameUp.ClientForm.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace FrameUp.ClientForm.Api.Controllers
{
    [ApiController]
    [Route("api/v1/account")]
    public class ClientFormController : ControllerBase
    {
        private readonly IAccountUseCase _accountUseCase;

        public ClientFormController(IAccountUseCase accountUseCase)
        {
            _accountUseCase = accountUseCase;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var response = await _accountUseCase.RegisterAsync(request);
            if (!response.IsSuccess)
                return BadRequest(response.Message);

            return Ok(response);
        }

        [HttpGet("exists")]
        public async Task<IActionResult> UserExists([FromQuery] string email)
        {
            var exists = await _accountUseCase.UserExistsAsync(email);
            if (!exists)
                return NotFound("User not found.");

            return Ok("User exists.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _accountUseCase.LoginAsync(request);
            if (!response.IsSuccess)
                return Unauthorized(response.Message);

            return Ok(response);
        }

        [HttpGet("validate")]
        public async Task<IActionResult> ValidateAccount([FromQuery] string email)
        {
            var isValid = await _accountUseCase.ValidateAccountAsync(email);
            if (!isValid)
                return BadRequest("Validation failed.");

            return Ok("Validation successful.");
        }
    }
}