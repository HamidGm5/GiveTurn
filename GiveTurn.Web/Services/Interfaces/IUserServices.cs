using GiveTurn.Models.Dto;

namespace GiveTurn.Blazor.Services.Interfaces
{
    public interface IUserServices
    {
        Task<UserDto> Login (string username, string password);
        Task<UserDto> SignUp(UserDto newuser);
        Task<UserDto> UpdateUser(UserDto newSpec);
        Task<bool> DeleteUser(int Userid);
    }
}
