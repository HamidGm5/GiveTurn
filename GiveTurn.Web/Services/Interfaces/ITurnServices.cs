using GiveTurn.Model.Dtos;

namespace GiveTurn.Blazor.Services.Interfaces
{
    public interface ITurnServices
    {
        Task<DateTime> GetTurnDateTime();
        Task<ICollection<TurnDto>> GetUserTurns(int Userid);
        Task<TurnDto> GetUserTurn(int Userid, int TurnId);
        Task<TurnDto> AddNewTurn(AddTurnDto addTurn);
        Task<TurnDto> DeleteTurn(int Userid,int TurnId);
        Task<TurnDto> DeleteAllUserTurns(int Userid);
    }
}
