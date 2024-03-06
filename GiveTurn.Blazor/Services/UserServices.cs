using GiveTurn.Blazor.Services.Interfaces;
using GiveTurn.Models.Dto;

namespace GiveTurn.Blazor.Services
{
    public class UserServices : IUserServices
    {
        public Task<UserDto> DeleteUser(int Userid)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> Login(string username, string password)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> SignUp(UserDto newuser)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> UpdateUser(UserDto newSpec)
        {
            throw new NotImplementedException();
        }
    }
}
