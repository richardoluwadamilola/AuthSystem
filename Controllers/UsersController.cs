using AuthSystem.Abstractions.IServices;
using AuthSystem.Models.DTOS;
using AuthSystem.Models.ResponseModels;
using AuthSystem.Utilities;
using Microsoft.AspNetCore.Authorization;
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
        [AllowAnonymous]
        [SkipGlobalAuthAction]
        public ActionResult<ServiceResponse<AuthResponseDTO>> Register([FromBody] RegisterDTO registerDto)
        {
            var result = userService.Register(registerDto.Username, registerDto.Email, registerDto.Password);
            if (result.HasError)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // Login a user
        [AllowAnonymous]
        [SkipGlobalAuthAction]
        [HttpPost("login")]
        public ActionResult<ServiceResponse<AuthResponseDTO>> Login([FromBody] LoginDTO loginDto)
        {
            var result = userService.Login(loginDto.Username, loginDto.Password);
            if (result.HasError)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        //Get user details
        [HttpGet("details")]
        [Authorize]
        public ActionResult<ServiceResponse<UserDTO>> GetUserDetails()
        {
            var username = userService.GetUserDetails();
            if (username.HasError)
            {
                return BadRequest(username);
            }
            return Ok(username);
        }
        



    }
}
