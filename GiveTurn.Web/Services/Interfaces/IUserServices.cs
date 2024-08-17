using GiveTurn.Model.Dtos;

namespace GiveTurn.Blazor.Services.Interfaces
{
    public interface IUserServices
    {
        Task<UserDto> Login(string username, string password);
        Task<UserDto> SignUp(UserDto newuser);
        Task<UserDto> UpdateUser(UserDto newSpec);
        Task<bool> UpdateUserHaveTurn(int userid, bool status);
        Task<bool> DeleteUser(int Userid);
    }
}
