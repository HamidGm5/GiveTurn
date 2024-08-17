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


        public async Task<bool> Delete(int Userid, int Turnid)
        {
            try
            {
                var FindTurn = await _context.Turns.Where(ut => ut.Id == Turnid &&
                                        ut.User.Id == Userid).FirstOrDefaultAsync();

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
            try
            {
                bool UserExist = await _context.Users.Where(ui => ui.Id == Userid).AnyAsync();
                if (!UserExist)
                {
                    return null;
                }
                else
                {
                    return await _context.Turns.Where(ut => ut.User.Id == Userid).ToListAsync();
                }
            }
            catch
            {
                return null;
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
                return DateTime.MinValue;
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

                if (LastTurn.Day >= Now.Day)
                {
                    DateTime LastTurnForReturn = LastTurn.AddMinutes(25);
                    TurnYear = LastTurnForReturn.Year;
                    TurnMonth = LastTurnForReturn.Month;
                    TurnDay = LastTurnForReturn.Day;

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
            DateTime DateTimeForReturn;

            if (ReserveDate.Hour >= 20)
            {
                DateTimeForReturn = new DateTime(ReserveDate.Year, ReserveDate.Month, ReserveDate.AddDays(1).Day,
                                                    8, 0, 0);
                return DateTimeForReturn;
            }
            else if (ReserveDate.Hour < 8)
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
            DateTime NowDateTime = DateTime.Now;
            DateTime NowDateTimePlus = NowDateTime.AddMinutes(25);
            DateTime ReserveDate = await CheckReserveDate();
            DateTime LastTurn = await LastTurnDateTime();
            DateTime LastTurnPlus = LastTurn.AddMinutes(25);

            DateTime DateTimeForReturn;

            var NowDateTimeToMili = new DateTimeOffset(NowDateTimePlus).ToUnixTimeMilliseconds();
            var LastDateTimeToMili = new DateTimeOffset(LastTurnPlus).ToUnixTimeMilliseconds();
            try
            {
                if (NowDateTimeToMili < LastDateTimeToMili)
                {
                    if (LastTurnPlus.Hour < 20)
                    {
                        DateTimeForReturn = new DateTime(LastTurnPlus.Year, LastTurnPlus.Month, LastTurnPlus.Day,
                                                            LastTurnPlus.Hour, LastTurnPlus.Minute, 0);
                        return DateTimeForReturn;
                    }
                    else
                    {
                        DateTimeForReturn = new DateTime(ReserveDate.Year, ReserveDate.Month, ReserveDate.Day,
                                          8, 0, 0);
                        return DateTimeForReturn;
                    }
                }
                else
                {
                    DateTimeForReturn = new DateTime(ReserveDate.Year, ReserveDate.Month, ReserveDate.Day,
                                                NowDateTimePlus.Hour, NowDateTimePlus.Minute, 0);
                    return DateTimeForReturn;
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

        public async Task<bool> DeleteAllUserTurns(int Userid)
        {
            try
            {
                var AllTurns = await _context.Turns.Where(ut => ut.User.Id == Userid).ToListAsync();
                foreach (var itemt in AllTurns)
                {
                    _context.Turns.Remove(itemt);
                }
                await _context.SaveChangesAsync();
                return true;
            }

            catch
            {
                return false;
            }
        }
    }//Class
}