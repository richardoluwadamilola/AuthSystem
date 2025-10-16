using AuthSystem.Abstractions.IRespositories;
using AuthSystem.Data;
using AuthSystem.Models;
using AuthSystem.Models.DTOS;
using AuthSystem.Models.ResponseModels;
using Microsoft.EntityFrameworkCore;

namespace AuthSystem.Implementations.Respositories
{
    public class UserRepository(ApplicationDBContext context) : IUserRepository
    {
        public Users CreateUser(Users user)
        {
            context.Users.Add(user);
            context.SaveChanges();
            return user;
        }

        public List<UserDTO> GetAllUsers()
        {
            return [.. context.Users.Select(u => new UserDTO
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                CreatedAt = u.CreatedAt
            })];
        }

        public UserDTO? GetUserByEmail(string email)
        {
            return context.Users.Where(u => u.Email == email).Select(u => new UserDTO
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                CreatedAt = u.CreatedAt
            }).FirstOrDefault();
        }

        public UserDTO? GetUserByUsername(string username)
        {
            return context.Users.Where(u => u.Username == username).Select(u => new UserDTO
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                CreatedAt = u.CreatedAt
            }).FirstOrDefault();
        }

        public Users? GetUserEntityByEmail(string email)
        {
            return context.Users.AsNoTracking().FirstOrDefault(u => u.Email == email);
        }

        public Users? GetUserEntityByUsername(string username)
        {
            return context.Users.AsNoTracking().FirstOrDefault(u => u.Username == username);
        }
    }
}
