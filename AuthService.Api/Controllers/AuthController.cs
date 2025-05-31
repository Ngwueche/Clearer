using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.API.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] SignDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(dto.UserName, dto.Password);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout(string userName)
        {
            var result = await _authService.LogOutAsync(userName);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RefreshTokenAsync(dto.UserName, dto.RefreshToken);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(dto);
            return Ok(result);
        }


        [HttpPost("deactivate-user")]
        public async Task<IActionResult> DeactivateUser(string username)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.DeactivateUserAsync(username);
            return Ok(result);
        }


        [HttpPost("delete-user")]
        public async Task<IActionResult> DeleteUser(string username)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.DeleteUserAsync(username);
            return Ok(result);
        }


        [HttpGet("get-roles")]
        public async Task<IActionResult> GetRoles()
        {
            var result = await _authService.GetRolesAsync();
            return Ok(result);
        }

    }
}
