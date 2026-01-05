using BikeStoreWeb.Core.DTOs;
using BikeStoreWeb.Core.Interfaces;
using BikeStoreWeb.Core.Responses;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;

namespace BikeStoreWeb.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")] // api/v1/auth
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse<AuthResponseDto>>> Register(RegisterDto registerDto)
        {
            var response = await _authService.RegisterAsync(registerDto);
            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }


        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<AuthResponseDto>>> Login(LoginDto loginDto)
        {
            var response = await _authService.LoginAsync(loginDto);
            if (!response.Success)
                return Unauthorized(response);

            return Ok(response);
        }

    }
}
