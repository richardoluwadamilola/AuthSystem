using AuthSystem.Abstractions.IRespositories;
using AuthSystem.Abstractions.IServices;
using AuthSystem.Models;
using AuthSystem.Models.DTOS;
using AuthSystem.Models.ResponseModels;
using AuthSystem.Utilities.Token;

namespace AuthSystem.Implementations.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly JwtSettings _jwtSettings;
        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _jwtSettings = new JwtSettings();
            _configuration.GetSection("JwtSettings").Bind(_jwtSettings);

        }
        public ServiceResponse<AuthResponseDTO> Login(string username, string password)
        {
            var response = new ServiceResponse<AuthResponseDTO>();

            try
            {
                var userResult = _userRepository.GetUserEntityByUsername(username);
                if (userResult == null || userResult.Data == null)
                {
                    response.Data = null;
                    response.HasError = true;
                    response.Message = "User not found";
                    return response;
                }

                var user = userResult.Data;

                bool passwordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
                if (!passwordValid)
                {
                    response.Data = null;
                    response.HasError = true;
                    response.Message = "Invalid password";
                    return response;
                }
                
                var token = TokenUtil.GetToken(user.Username, user.Email);
                response.Data = new AuthResponseDTO
                {
                    Token = token,
                    User = new UserDTO 
                    { 
                        Id = user.Id, 
                        Username = user.Username, 
                        Email = user.Email, 
                        CreatedAt = user.CreatedAt
                    }
                };
                response.HasError = false;
                response.Message = "Login successful";
            }
            catch (Exception ex)
            {
                response.Data = null;
                response.HasError = true;
                response.Message = $"Error during login: {ex.Message}";
            }

            return response;
        }

        public ServiceResponse<AuthResponseDTO> Register(string username, string email, string password)
        {
            var response = new ServiceResponse<AuthResponseDTO>();

            try
            {
                var existingUserByUsername = _userRepository.GetUserEntityByUsername(username);
                if (existingUserByUsername != null && existingUserByUsername.Data != null)
                {
                    response.Data = null;
                    response.HasError = true;
                    response.Message = "Username already exists";
                    return response;
                }
                var existingUserByEmail = _userRepository.GetUserByEmail(email);
                if (existingUserByEmail != null && existingUserByEmail.Data != null)
                {
                    response.Data = null;
                    response.HasError = true;
                    response.Message = "Email already exists";
                    return response;
                }
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
                var newUser = new Users
                {
                    Username = username,
                    Email = email,
                    PasswordHash = hashedPassword,
                    CreatedAt = DateTime.UtcNow
                };
                var createResult = _userRepository.CreateUser(newUser);
                if (createResult.HasError)
                {
                    response.Data = null;
                    response.HasError = true;
                    response.Message = createResult.Message;
                    return response;
                }
                var token = TokenUtil.GetToken(username, email);
                response.Data = new AuthResponseDTO
                {
                    Token = token,
                    User = new UserDTO
                    {
                        Username = username,
                        Email = email,
                        CreatedAt = newUser.CreatedAt
                    }
                };
                response.HasError = false;
                response.Message = "Registration successful";
            }
            catch (Exception ex)
            {
                response.Data = null;
                response.HasError = true;
                response.Message = $"Error during registration: {ex.Message}";
            }

            return response;

        }
    }
}
