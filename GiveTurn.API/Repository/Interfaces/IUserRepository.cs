using GiveTurn.API.Entities;

namespace GiveTurn.API.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<ICollection<User>> GetAllUsers();
        Task<User> GetUserById(int Userid);
        Task<User> Login(string username, string password);
        Task<User> SignUp(User user);
        Task<User> Update(int Userid, User user);
        Task<bool> Delete(int Userid);
    }
}
