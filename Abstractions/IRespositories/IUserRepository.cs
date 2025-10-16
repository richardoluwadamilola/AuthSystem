using AuthSystem.Data;
using AuthSystem.Models;
using AuthSystem.Models.DTOS;
using AuthSystem.Models.ResponseModels;
using Microsoft.EntityFrameworkCore;

namespace AuthSystem.Abstractions.IRespositories
{
    public interface IUserRepository
    {
        UserDTO? GetUserByUsername(string username);
        UserDTO? GetUserByEmail(string email);
        List<UserDTO> GetAllUsers();
        Users CreateUser(Users user);
        Users? GetUserEntityByUsername(string username);
        Users? GetUserEntityByEmail(string email);
    }

    
}
