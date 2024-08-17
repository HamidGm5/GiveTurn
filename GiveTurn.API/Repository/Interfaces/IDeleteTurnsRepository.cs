using GiveTurn.API.Entities;

namespace GiveTurn.API.Repository.Interfaces
{
    public interface IDeleteTurnsRepository
    {
        Task<bool> UserDeletedTurn(int UserID);
        Task<bool> AddDeleteTurn(DeleteTurns turn);
        Task DeleteTurns();
    }
}
