using GiveTurn.Model.Dtos;

namespace GiveTurn.Blazor.Services.Interfaces
{
    public interface ITurnServices
    {
        Task<DateTime> GetTurnDateTime();
        Task<ICollection<TurnDto>> GetUserTurns(int Userid);
        Task<TurnDto> UserLastTurn(int Userid);
        Task<TurnDto> GetUserTurn(int Userid, int TurnId);
        Task<TurnDto> AddNewTurn(AddTurnDto addTurn);
        Task<bool> DeleteTurn(int Userid, int TurnId);
        Task<bool> DeleteAllUserTurns(int Userid);
    }
}
