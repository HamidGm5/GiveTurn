using GiveTurn.Blazor.Services.Interfaces;
using GiveTurn.Models.Dto;

namespace GiveTurn.Blazor.Services
{
    public class TurnServices : ITurnServices
    {
        public Task<TurnDto> DeleteTurn(int TurnId)
        {
            throw new NotImplementedException();
        }

        public Task<TurnDto> GetAddNewTurn(TurnDto turn)
        {
            throw new NotImplementedException();
        }

        public Task<TurnDto> GetUserTurn(int Userid, int TurnId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<TurnDto>> GetUserTurns(int Userid)
        {
            throw new NotImplementedException();
        }
    }
}
