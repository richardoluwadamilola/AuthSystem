using AuthSystem.Abstractions.IRespositories;
using AuthSystem.Abstractions.IServices;
using AuthSystem.Models;
using AuthSystem.Models.DTOS;
using AuthSystem.Models.ResponseModels;

namespace AuthSystem.Implementations.Services
{
    public class UserService(IUserRepository userRepository, ITokenService tokenService) : IUserService
    {
        public ServiceResponse<AuthResponseDTO> Login(string username, string password)
        {
            var response = new ServiceResponse<AuthResponseDTO>();

            try
            {
                var userResult = userRepository.GetUserEntityByUsername(username);
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
                
                response.Data = new AuthResponseDTO
                {
                    Token = tokenService.GenerateToken(user.Username, user.Email),
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
                var existingUserByUsername = userRepository.GetUserByUsername(username);
                if (existingUserByUsername != null && existingUserByUsername.Data != null)
                {
                    response.Data = null;
                    response.HasError = true;
                    response.Message = "Username already exists";
                    return response;
                }
                var existingUserByEmail = userRepository.GetUserByEmail(email);
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
                    CreatedAt = DateTime.Now
                };
                var createResult = userRepository.CreateUser(newUser);
                if (createResult.HasError)
                {
                    response.Data = null;
                    response.HasError = true;
                    response.Message = createResult.Message;
                    return response;
                }
                
                response.Data = new AuthResponseDTO
                {
                    Token = tokenService.GenerateToken(username,email),
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
