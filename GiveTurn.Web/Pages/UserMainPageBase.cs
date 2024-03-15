using Blazored.Toast.Services;
using GiveTurn.Blazor.Services.Interfaces;
using GiveTurn.Models.Dto;
using Microsoft.AspNetCore.Components;

namespace GiveTurn.Web.Pages
{
    public class UserMainPageBase : ComponentBase
    {
        [Parameter]
        public string Username { get; set; }
        [Parameter]
        public string Password { get; set; }

        [Inject]
        public IUserServices UserServices { get; set; }
        [Inject]
        public ITurnServices TurnServices { get; set; }
        [Inject]
        public IToastService Toast { get; set; }

        public UserDto User { get; set; }
        public ICollection<TurnDto> Turns { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime TodayTurn { get; set; }
        public bool IsTurnToday { get; set; } = false;
        protected override async Task OnParametersSetAsync()
        {
            try
            {
                DateTime Now = DateTime.Now;
                User = await UserServices.Login(Username, Password);
                Turns = await TurnServices.GetUserTurns(User.Id);
                foreach (var turn in Turns)
                {
                    if (turn.UserTurnDate.Day == Now.Day && turn.UserTurnDate.Hour < Now.Hour)
                    {
                        IsTurnToday = true;
                        TodayTurn = turn.UserTurnDate;
                    }
                    else
                    {
                        IsTurnToday = false;
                    }
                }
            }

            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}
