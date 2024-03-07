using GiveTurn.Models.Dto;

namespace GiveTurn.Blazor.Services.Interfaces
{
    public interface ITurnServices
    {
        Task<ICollection<TurnDto>> GetUserTurns(int Userid);
        Task<TurnDto> GetUserTurn(int Userid, int TurnId);
        Task<TurnDto> GetAddNewTurn(TurnDto turn);
        Task<TurnDto> DeleteTurn(int TurnId);
    }
}
