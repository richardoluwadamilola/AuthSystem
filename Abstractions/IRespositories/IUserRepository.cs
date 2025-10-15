using AuthSystem.Data;
using AuthSystem.Models;
using AuthSystem.Models.DTOS;
using AuthSystem.Models.ResponseModels;
using Microsoft.EntityFrameworkCore;

namespace AuthSystem.Abstractions.IRespositories
{
    public interface IUserRepository
    {
        ServiceResponse<UserDTO> GetUserByUsername(string username);
        ServiceResponse<UserDTO> GetUserByEmail(string email);
        List<UserDTO> GetAllUsers();
        ServiceResponse<string> CreateUser(Users user);
        ServiceResponse<Users> GetUserEntityByUsername(string username);
        ServiceResponse<Users> GetUserEntityByEmail(string email);
    }

    
}
