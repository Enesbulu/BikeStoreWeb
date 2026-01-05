using BikeStoreWeb.Core.DTOs;
using BikeStoreWeb.Core.Interfaces;
using BikeStoreWeb.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BikeStoreWeb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
