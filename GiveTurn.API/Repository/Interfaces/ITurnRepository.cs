using GiveTurn.API.Entities;

namespace GiveTurn.API.Repository.Interfaces
{
    public interface ITurnRepository
    {
        Task<ICollection<Turn>> GetAllTurns();
        Task<DateTime> LastTurnDateTime();
        Task<Turn> GetTurnById(int Turnid);
        Task<ICollection<Turn>> GetUserTurns(int Userid);
        Task<Turn> GetUserTurn(int Userid, int Turnid);
        Task<Turn> AddTurns(Turn turn);
        Task<Turn> Update(int Turnid, Turn UserTurn);
        Task<bool> Delete(int Turnid);


        Task<DateTime> GiveTurnDateTime();
        Task<DateTime> CheckDateForTurn();
        Task<DateTime> CheckTime();
    }
}
