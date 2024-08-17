using GiveTurn.API.Context;
using GiveTurn.API.Entities;
using GiveTurn.API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GiveTurn.API.Repository
{
    public class DeletedTurnRepository : IDeleteTurnsRepository
    {
        private readonly GiveTurnContext _context;

        public DeletedTurnRepository(GiveTurnContext context)
        {
            _context = context;
        }

        public async Task<bool> AddDeleteTurn(DeleteTurns turn)
        {
            try
            {
                var add = await _context.deleteTurns.AddAsync(turn);
                if (add != null)
                {
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task DeleteTurns()
        {
            try
            {
                var AllTurnsForBefor = await _context.deleteTurns.Where(bt => bt.TurnDate < DateTime.Now).ToListAsync();
                _context.deleteTurns.RemoveRange(AllTurnsForBefor);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<bool> UserDeletedTurn(int UserID)
        {
            try
            {
                return await _context.deleteTurns.Where(ut => ut.Userid == UserID).FirstOrDefaultAsync() != null;
            }
            catch
            {
                Console.WriteLine("somthing get wrong in DeletedTurnsRepository !");
                return false;
            }
        }
    }
}
