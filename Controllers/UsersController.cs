using AuthSystem.Abstractions.IServices;
using AuthSystem.Models.DTOS;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserService userService) : ControllerBase
    {

        // Register a new user
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDTO registerDto)
        {
            var result = userService.Register(registerDto.Username, registerDto.Email, registerDto.Password);
            if (result.HasError)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // Login a user
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO loginDto)
        {
            var result = userService.Login(loginDto.Username, loginDto.Password);
            if (result.HasError)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }



    }
}
