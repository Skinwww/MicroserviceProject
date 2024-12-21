using AuthApiSolution.Application.DTOs;
using ECommercelib.SharedLibrary.Responses;

namespace AuthApiSolution.Application.Interfaces
{
    public interface IUser
    {
        Task<Response> Register(AppUserDTO appUserDTO);
        Task<Response> Login(LoginDTO loginDTO);
        Task<GetUserDTO> GetUser(int userId);
    }
}
