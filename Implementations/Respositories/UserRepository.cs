using AuthSystem.Abstractions.IRespositories;
using AuthSystem.Data;
using AuthSystem.Models;
using AuthSystem.Models.DTOS;
using AuthSystem.Models.ResponseModels;
using Microsoft.EntityFrameworkCore;

namespace AuthSystem.Implementations.Respositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDBContext _context;

        public UserRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public ServiceResponse<string> CreateUser(Users user)
        {
            var response = new ServiceResponse<string>();

            try
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                response.Data = "User created successfully";
                response.HasError = false;
                response.Message = "User created successfully";
            }
            catch (Exception ex)
            {
                response.Data = null;
                response.HasError = true;
                response.Message = $"Error creating user: {ex.Message}";
            }

            return response;
        }

        public List<UserDTO> GetAllUsers()
        {
            return _context.Users.Select(u => new UserDTO
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                CreatedAt = u.CreatedAt
            }).ToList();
        }

        public ServiceResponse<UserDTO> GetUserByEmail(string email)
        {
            var response = new ServiceResponse<UserDTO>();

            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == email);
                if (user != null)
                {
                    response.Data = new UserDTO
                    {
                        Id = user.Id,
                        Username = user.Username,
                        Email = user.Email,
                        CreatedAt = user.CreatedAt
                    };
                    response.HasError = false;
                    response.Message = "User found";
                }
                else
                {
                    response.Data = null;
                    response.HasError = true;
                    response.Message = "User not found";
                }
            }
            catch (Exception ex)
            {
                response.Data = null;
                response.HasError = true;
                response.Message = $"Error retrieving user: {ex.Message}";
            }

            return response;

        }

        public ServiceResponse<UserDTO> GetUserByUsername(string username)
        {
            var response = new ServiceResponse<UserDTO>();

            try             
            {
                var user = _context.Users.FirstOrDefault(u => u.Username == username);
                if (user != null)
                {
                    response.Data = new UserDTO
                    {
                        Id = user.Id,
                        Username = user.Username,
                        Email = user.Email,
                        CreatedAt = user.CreatedAt
                    };
                    response.HasError = false;
                    response.Message = "User found";
                }
                else
                {
                    response.Data = null;
                    response.HasError = true;
                    response.Message = "User not found";
                }
            }
            catch (Exception ex)
            {
                response.Data = null;
                response.HasError = true;
                response.Message = $"Error retrieving user: {ex.Message}";
            }

            return response;
        }

        public ServiceResponse<Users> GetUserEntityByEmail(string email)
        {
            var response = new ServiceResponse<Users>();

            try
            {
                var user = _context.Users.AsNoTracking().FirstOrDefault(u => u.Email == email);
                if (user != null)
                {
                    response.Data = user;
                    response.HasError = false;
                    response.Message = "User found";
                }
                else
                {
                    response.Data = null;
                    response.HasError = true;
                    response.Message = "User not found";
                }
            }
            catch (Exception ex)
            {
                response.Data = null;
                response.HasError = true;
                response.Message = $"Error retrieving user: {ex.Message}";
            }

            return response;
        }

        public ServiceResponse<Users> GetUserEntityByUsername(string username)
        {
            var response = new ServiceResponse<Users>();

            try
            {
                var user = _context.Users.AsNoTracking().FirstOrDefault(u => u.Username == username);
                if (user != null)
                {
                    response.Data = user;
                    response.HasError = false;
                    response.Message = "User found";
                }
                else
                {
                    response.Data = null;
                    response.HasError = true;
                    response.Message = "User not found";
                }
            }
            catch (Exception ex)
            {
                response.Data = null;
                response.HasError = true;
                response.Message = $"Error retrieving user: {ex.Message}";
            }

            return response;
        }
    }
}
