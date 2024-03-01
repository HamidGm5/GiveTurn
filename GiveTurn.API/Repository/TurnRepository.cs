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
                var LastTurnDateTimeWithPlus = LastTurn.AddMinutes(25);
                //var NowWithPlus = Now.AddMinutes(25);

                if (LastTurnDateTimeWithPlus.Hour > 20)             //First Check Date Time Now With Last Turn 
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


        public async Task<DateTime> CheckTime()
        {
            DateTime Now = DateTime.Now;
            var DateFromCheckDate = await CheckDateForTurn();
            DateTime TurnTime;

            int TurnHour, TurnMinute;

            if (DateFromCheckDate == null)
            {
                TurnTime = Now.AddMinutes(10);
                TurnHour = TurnTime.Hour;
                TurnMinute = TurnTime.Minute;

                TurnTime = new DateTime(DateFromCheckDate.Year, DateFromCheckDate.Month, DateFromCheckDate.Day,
                                        TurnHour, TurnMinute, 0);

                return TurnTime;
            }

            else
            {
                var PlusNow = Now.AddMinutes(15);
                var PlusNowToMili = new DateTimeOffset(PlusNow).ToUnixTimeMilliseconds();
                var LastTurnToMili = new DateTimeOffset(DateFromCheckDate).ToUnixTimeMilliseconds();

                if (PlusNowToMili > LastTurnToMili)         // Here it's Not Good Condition!
                {
                    TurnHour = PlusNow.Hour;
                    TurnMinute = PlusNow.Minute;


                    TurnTime = new DateTime(DateFromCheckDate.Year, DateFromCheckDate.Month, DateFromCheckDate.Day,
                                            TurnHour, TurnMinute, 0);

                    return TurnTime;
                }

                else
                {
                    var PlusToLast = DateFromCheckDate.AddMinutes(10);
                    TurnHour = PlusToLast.Hour;
                    TurnMinute = PlusToLast.Minute;

                    TurnTime = new DateTime(DateFromCheckDate.Year, DateFromCheckDate.Month, DateFromCheckDate.Day,
                        TurnHour, TurnMinute, 0);

                    return TurnTime;
                }
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
    }// Class
}