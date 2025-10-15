using AuthSystem.Models.DTOS;
using AuthSystem.Models.ResponseModels;

namespace AuthSystem.Abstractions.IServices
{
    public interface IUserService
    {
      ServiceResponse<AuthResponseDTO> Register(string username, string email, string password);
      ServiceResponse<AuthResponseDTO> Login(string username, string password);

    }
}
