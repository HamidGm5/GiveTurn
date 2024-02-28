using GiveTurn.API.Context;
using GiveTurn.API.Entities;
using GiveTurn.API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GiveTurn.API.Repository
{
    public class TurnRepository : ITurnRepository
    {
        private readonly GiveTurnContext _context;

        public TurnRepository(GiveTurnContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Turn>> GetAllTurns()
        {
            try
            {
                var Turns = await _context.Turns.ToListAsync();
                if (Turns != null)
                {
                    return Turns;
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

        public async Task<Turn> AddTurns(Turn turn)
        {
            try
            {
                await _context.Turns.AddAsync(turn);
                await _context.SaveChangesAsync();
                return turn;
            }

            catch
            {
                return null;
            }
        }


        public async Task<bool> Delete(int Turnid)
        {
            try
            {
                var FindTurn = await GetTurnById(Turnid);

                if (FindTurn != null)
                {
                    _context.Turns.Remove(FindTurn);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }

            catch
            {
                return false;
            }
        }


        public async Task<Turn> GetTurnById(int Turnid)
        {
            try
            {
                var Turn = await _context.Turns.FindAsync(Turnid);

                if (Turn != null)
                {
                    return Turn;
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


        public async Task<User> GetUserById(int Userid)
        {
            try
            {
                var User = await _context.Users.FindAsync(Userid);

                if (User == null)
                {
                    return null;
                }
                else
                {
                    return User;
                }
            }
            catch
            {
                return null;
            }
        }


        public async Task<Turn> GetUserTurn(int Userid, int Turnid)
        {
            var User = await GetUserById(Userid);

            if (User == null)
            {
                return null;
            }
            else
            {
                return await _context.Turns.Where(ut => ut.User == User && ut.Id == Turnid).FirstOrDefaultAsync();
            }
        }


        public async Task<ICollection<Turn>> GetUserTurns(int Userid)
        {
            var User = await GetUserById(Userid);

            if (User == null)
            {
                return null;
            }
            else
            {
                return await _context.Turns.Where(ut => ut.User == User).ToListAsync();
            }
        }


        public async Task<Turn> Update(int Turnid, Turn UserTurn)
        {
            try
            {
                var FindTurn = await GetTurnById(Turnid);

                if (FindTurn == null)
                {
                    return null;
                }
                else
                {
                    FindTurn.UserTurnDate = UserTurn.UserTurnDate;
                    return FindTurn;
                }
            }
            catch
            {
                return null;
            }
        }


        public async Task<DateTime> GiveTurnDateTime()
        {
            try
            {
                DateTime now = DateTime.Now;
                DateTime TurnDateTime;

                var Date = await CheckDateForTurn();

                int TurnDay = Date.Day;

                var Time = CheckTime(TurnDay);

                TurnDateTime = new DateTime(Date.Year, Date.Month, Date.Day, Time[0], Time[1], 0);

                return TurnDateTime;
            }

            catch
            {
                return default(DateTime);
            }
        }


        public async Task<DateTime> CheckDateForTurn()
        {
            try
            {
                DateTime Now = DateTime.Now;
                int TurnYear, TurnMonth, TurnDay;
                DateTime TurnDateForReturn;

                DateTime LastTurn = await LastTurnDateTime();
                var LastTurnDateTimeWithPlus = LastTurn.AddMinutes(25);

                if (LastTurnDateTimeWithPlus.Hour > 20)
                {
                    var TurnDate = LastTurnDateTimeWithPlus.AddDays(1);
                    TurnYear = LastTurnDateTimeWithPlus.Year;
                    TurnMonth = LastTurnDateTimeWithPlus.Month;
                    TurnDay = TurnDate.Day;

                    TurnDateForReturn = new DateTime(TurnYear, TurnMonth, TurnDay);
                    return TurnDateForReturn;
                }

                else
                {
                    TurnYear = Now.Year;
                    TurnMonth = Now.Month;
                    TurnDay = Now.Day;

                    TurnDateForReturn = new DateTime(TurnYear, TurnMonth, TurnDay);
                    return TurnDateForReturn;
                }
            }
            catch
            {
                return DateTime.MinValue;
            }
        }


        public int[] CheckTime(int TurnDay)
        {
            DateTime Now = DateTime.Now;
            int[] HourAndMinut = new int[2];
            DateTime TurnTime = Now;

            var ConvertDay = Convert.ToInt32(TurnDay);
            int TurnHour, TurnMinute = 0;
            var LastTurn = _context.Turns.Where(ut => ut.UserTurnDate.Day == ConvertDay).Last();

            if (LastTurn == null)
            {
                TurnTime = Now.AddMinutes(10);
                TurnHour = TurnTime.Hour;
                TurnMinute = TurnTime.Minute;
                HourAndMinut[0] = TurnHour;
                HourAndMinut[1] = TurnMinute;

                return HourAndMinut;
            }

            else
            {
                var PlusNow = Now.AddMinutes(15);
                var PlusNowToMili = new DateTimeOffset(PlusNow).ToUnixTimeMilliseconds();
                var LastTurnToMili = new DateTimeOffset(LastTurn.UserTurnDate).ToUnixTimeMilliseconds();

                if (PlusNowToMili > LastTurnToMili)
                {
                    HourAndMinut[0] = PlusNow.Hour;
                    HourAndMinut[1] = PlusNow.Minute;

                    return HourAndMinut;
                }

                else
                {
                    var PlusToLast = LastTurn.UserTurnDate.AddMinutes(10);
                    HourAndMinut[0] = PlusToLast.Hour;
                    HourAndMinut[1] = PlusToLast.Minute;

                    return HourAndMinut;
                }
            }
        }


        public async Task<DateTime> LastTurnDateTime()
        {
            try
            {
                var LastTurn = await _context.Turns.LastOrDefaultAsync();
                if (LastTurn != null)
                {
                    return LastTurn.UserTurnDate;
                }
                else
                {
                    return DateTime.MinValue;
                }
            }

            catch
            {
                return DateTime.MinValue;
            }
        }
    }// Class
}