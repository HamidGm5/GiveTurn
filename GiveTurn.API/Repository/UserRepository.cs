using GiveTurn.API.Context;
using GiveTurn.API.Entities;
using GiveTurn.API.Repository.Interfaces;
using GiveTurn.Model.Dtos;
using Microsoft.EntityFrameworkCore;

namespace GiveTurn.API.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly GiveTurnContext _context;
        public UserRepository(GiveTurnContext context)
        {
            _context = context;
        }
        public async Task<bool> Delete(int Userid)
        {
            try
            {
                var User = await GetUserById(Userid);

                if (User == null)
                {
                    return false;
                }
                else
                {
                    var UserTurns = await _context.Turns.Where(ut => ut.User.Id == Userid).ToListAsync();
                    foreach (var turn in UserTurns)
                    {
                        _context.Turns.Remove(turn);
                        await _context.SaveChangesAsync();
                    }
                    _context.Users.Remove(User);
                    await _context.SaveChangesAsync();
                    return true;
                }
            }

            catch
            {
                return false;
            }
        }

        public async Task<ICollection<User>> GetAllUsers()
        {
            try
            {
                var users = await _context.Users.ToListAsync();
                return users;
            }
            catch
            {
                return null;
            }
        }

        public async Task<User> GetUserById(int Userid)
        {
            try
            {
                var FindUser = await _context.Users.FindAsync(Userid);
                if (FindUser == null)
                {
                    return null;
                }
                else
                {
                    return FindUser;
                }
            }

            catch
            {
                return null;
            }
        }

        public async Task<User> Login(string username, string password)
        {
            try
            {
                var User = await _context.Users.Where(u => u.Username.Trim().ToLower() ==
                                                    username.Trim().ToLower()).FirstOrDefaultAsync();

                if (User == null)
                {
                    return null;
                }
                else
                {
                    if (User.Password == password)
                    {
                        return User;
                    }

                    else
                    {
                        return null;
                    }
                }
            }

            catch
            {
                return null;
            }
        }

        public async Task<User> SignUp(User user)
        {
            try
            {
                var adduser = await _context.Users.AddAsync(user);
                if (adduser != null)
                {
                    await _context.SaveChangesAsync();
                    return user;
                }
                else
                {
                    return null;
                }
            }

            catch
            {
                return null;
            }
        }

        public async Task<User> Update(int Userid, User user)
        {
            try
            {
                var FindUser = await GetUserById(Userid);

                if (FindUser != null)
                {
                    FindUser.Username = user.Username;
                    FindUser.Password = user.Password;
                    FindUser.PhoneNumber = user.PhoneNumber;
                    FindUser.HaveTurn = user.HaveTurn;
                    await _context.SaveChangesAsync();
                    return FindUser;
                }

                else
                {
                    return null;
                }
            }

            catch
            {
                return null;
            }
        }
    }
}
