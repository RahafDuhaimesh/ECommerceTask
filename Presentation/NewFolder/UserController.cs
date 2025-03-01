using ECommerceTask.Application.DTOs;
using ECommerceTask.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerceTask.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterReqDTO model)
        {
            var result = await _userService.RegisterUserAsync(model);
            return Ok(new { message = result });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginReqDTO model)
        {
            var result = await _userService.LoginUserAsync(model);
            if (result == null || string.IsNullOrEmpty(result.Token))
                return Unauthorized(new { message = result?.Message ?? "Invalid credentials" });

            return Ok(result);
        }
    }
}
