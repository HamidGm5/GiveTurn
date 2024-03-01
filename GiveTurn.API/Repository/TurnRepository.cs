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

        public async Task<Turn> GetUserTurn(int Userid, int Turnid)
        {
            var User = await _context.Users.Where(ui => ui.Id == Userid).FirstOrDefaultAsync();

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
            var User = await _context.Users.Where(ui => ui.Id == Userid).FirstOrDefaultAsync();

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
                DateTime TurnDateTime = await CheckTime();

                return TurnDateTime;
            }

            catch
            {
                return DateTime.MinValue;           // Somthing Not Work and We are Have Problem Here
            }
        }


        public async Task<DateTime> CheckDateForTurn()
        {
            try
            {
                DateTime Now = DateTime.Now;
                int TurnYear, TurnMonth, TurnDay;
                DateTime TurnDateForReturn;

                DateTime LastTurn = await LastTurnDateTime();           //Check For Max Value

                if (LastTurn.Day >= Now.Day)
                {
                    TurnYear = LastTurn.Year;
                    TurnMonth = LastTurn.Month;
                    TurnDay = LastTurn.Day;

                    TurnDateForReturn = new DateTime(TurnYear, TurnMonth, TurnDay, LastTurn.Hour, LastTurn.Minute, 0);
                    return TurnDateForReturn;
                }
                else
                {
                    TurnYear = Now.Year;
                    TurnMonth = Now.Month;
                    TurnDay = Now.Day;

                    DateTime CheckTime = LastTurn.AddMinutes(25);
                    TurnDateForReturn = new DateTime(TurnYear, TurnMonth, TurnDay, CheckTime.Hour, CheckTime.Minute, 0);
                    return TurnDateForReturn;
                }
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        public async Task<DateTime> CheckReserveDate()
        {
            DateTime ReserveDate = await CheckDateForTurn();
            DateTime ReserveDateWithPlus = ReserveDate.AddMinutes(25);
            DateTime DateTimeForReturn;

            if (ReserveDateWithPlus.Hour >= 20)
            {
                DateTimeForReturn = ReserveDate.AddDays(1);
                return DateTimeForReturn;
            }
            else if (ReserveDateWithPlus.Hour < 8)
            {
                DateTimeForReturn = new DateTime(ReserveDate.Year, ReserveDate.Month, ReserveDate.Day,
                                                    8, 0, 0);
                return DateTimeForReturn;
            }
            else
            {
                return ReserveDate;
            }
        }

        public async Task<DateTime> CheckTime()
        {
            int Hour;
            int Minute;
            DateTime ReserveDate = await CheckReserveDate();
            DateTime LastTurn = await LastTurnDateTime();
            DateTime DateTimeForReturn;

            try
            {
                var NowPlusToMili = new DateTimeOffset(DateTime.Now.AddMinutes(25)).ToUnixTimeMilliseconds();
                var LastToMili = new DateTimeOffset(LastTurn).ToUnixTimeMilliseconds();

                if (LastToMili > NowPlusToMili)
                {
                    DateTime NextLastTurn = LastTurn.AddMinutes(25);
                    DateTimeForReturn = new DateTime(NextLastTurn.Year, NextLastTurn.Month, NextLastTurn.Day,
                                                        NextLastTurn.Hour, NextLastTurn.Minute, 0);
                    return DateTimeForReturn;
                }
                else
                {
                    DateTime ReserveDateWithPlus = ReserveDate.AddMinutes(25);
                    DateTimeForReturn = new DateTime(ReserveDateWithPlus.Year, ReserveDateWithPlus.Month, ReserveDateWithPlus.Day,
                                                        ReserveDateWithPlus.Hour, ReserveDateWithPlus.Minute, 0);
                    return ReserveDateWithPlus;
                }
            }

            catch
            {
                return DateTime.MinValue;
            }
        }


        public async Task<DateTime> LastTurnDateTime()
        {
            try
            {
                var LastTurn = await _context.Turns.OrderByDescending(i => i.Id).FirstOrDefaultAsync();
                if (LastTurn != null)
                {
                    return LastTurn.UserTurnDate;
                }
                else
                {
                    return DateTime.Now;
                }
            }

            catch
            {
                return DateTime.MinValue;
            }
        }

    }//Class
}