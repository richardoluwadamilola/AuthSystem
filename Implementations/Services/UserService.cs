using AuthSystem.Abstractions.IRespositories;
using AuthSystem.Abstractions.IServices;
using AuthSystem.Models;
using AuthSystem.Models.DTOS;
using AuthSystem.Models.ResponseModels;

namespace AuthSystem.Implementations.Services
{
    public class UserService(IUserRepository userRepository, ITokenService tokenService, ITokenAccessor tokenAccessor) : IUserService
    {
        public ServiceResponse<UserDTO> GetUserDetails()
        {
            var userName = tokenAccessor.GetAuthenticatedTokenUsername();
            if (string.IsNullOrEmpty(userName))
            {
                return new ServiceResponse<UserDTO>
                {
                    Data = null,
                    HasError = true,
                    Message = "Invalid token or user not found"
                };
            }

            var user = userRepository.GetUserByUsername(userName);
            if (user == null)
            {
                return new ServiceResponse<UserDTO>
                {
                    Data = null,
                    HasError = true,
                    Message = "User not found"
                };
            }

            return new ServiceResponse<UserDTO>
            {
                Data = user,
                HasError = false,
                Message = "User details retrieved successfully",
                HttpStatusCode = System.Net.HttpStatusCode.OK
            };
        }

        public ServiceResponse<AuthResponseDTO> Login(string username, string password)
        {
            var userEntity = userRepository.GetUserEntityByUsername(username);
            if (userEntity == null)
            {
                return new ServiceResponse<AuthResponseDTO>
                {
                    Data = null,
                    HasError = true,
                    Message = "User not found",
                    HttpStatusCode = System.Net.HttpStatusCode.NotFound
                };
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, userEntity.PasswordHash);
            if (!isPasswordValid)
            {
                return new ServiceResponse<AuthResponseDTO>
                {
                    Data = null,
                    HasError = true,
                    Message = "Invalid password",
                    HttpStatusCode = System.Net.HttpStatusCode.BadRequest
                };
            }

            var token = tokenService.GenerateToken(userEntity.Username, userEntity.Email);
            var authResponse = new AuthResponseDTO
            {
                Token = token,
                User = new UserDTO
                {
                    Id = userEntity.Id,
                    Username = userEntity.Username,
                    Email = userEntity.Email,
                    CreatedAt = userEntity.CreatedAt
                }
            };
            return new ServiceResponse<AuthResponseDTO>
            {
                Data = authResponse,
                HasError = false,
                Message = "Login successful",
                HttpStatusCode = System.Net.HttpStatusCode.OK
            };

        }

        public ServiceResponse<AuthResponseDTO> Register(string username, string email, string password)
        {
            var exisingUserByUsername = userRepository.GetUserByUsername(username);
            if (exisingUserByUsername != null)
            {
                return new ServiceResponse<AuthResponseDTO>
                {
                    Data = null,
                    HasError = true,
                    Message = "Username already taken",
                    HttpStatusCode = System.Net.HttpStatusCode.BadRequest
                };
            }

            var existingUserByEmail = userRepository.GetUserByEmail(email);
            if (existingUserByEmail != null)
            {
                return new ServiceResponse<AuthResponseDTO>
                {
                    Data = null,
                    HasError = true,
                    Message = "Email already registered",
                    HttpStatusCode = System.Net.HttpStatusCode.BadRequest
                };
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            var newUser = new Users
            {
                Username = username,
                Email = email,
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow
            };

            var createdUser = userRepository.CreateUser(newUser);

            var token = tokenService.GenerateToken(newUser.Username, newUser.Email);

            var authResponse = new AuthResponseDTO
            {
                Token = token,
                User = new UserDTO
                {
                    Id = createdUser.Id,
                    Username = newUser.Username,
                    Email = newUser.Email,
                    CreatedAt = newUser.CreatedAt
                }
            };
            return new ServiceResponse<AuthResponseDTO>
            {
                Data = authResponse,
                HasError = false,
                Message = "Registration successful",
                HttpStatusCode = System.Net.HttpStatusCode.OK
            };
        }
    }
}
