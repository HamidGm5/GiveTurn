using Blazored.Toast.Services;
using GiveTurn.Blazor.Services.Interfaces;
using GiveTurn.Model.Dtos;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

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
        [Inject]
        public IJSRuntime Js { get; set; }
        [Inject]
        public NavigationManager Navigate { get; set; }
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
                var LastTurn = Turns.LastOrDefault();

                if (Turns != null)
                {
                    if ((LastTurn.UserTurnDate.Year != Now.Year ||
                        LastTurn.UserTurnDate.Month != Now.Month ||
                        LastTurn.UserTurnDate.Day != Now.Day) &&
                        User.HaveTurn == true)
                    {
                        await UserServices.UpdateUserHaveTurn(User.Id, false);
                    }
                }
                foreach (var turn in Turns)
                {
                    if (turn.UserTurnDate < Now.AddHours(24) && turn.UserTurnDate > Now)
                    {
                        IsTurnToday = true;
                        TodayTurn = turn.UserTurnDate;
                    }
                    else if (turn.UserTurnDate < Now)
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

        public void GoToGiveTurnPage_Click()
        {
            string GiveTurnUrl = $"GiveTurn/{Username}/{Password}";
            Navigate.NavigateTo(GiveTurnUrl);
        }

        public void GoToSetting_Click()
        {
            string SettingUrl = $"/Setting/{Username}/{Password}";
            Navigate.NavigateTo(SettingUrl);
        }

        public async void DeleteTurn(int TurnId)
        {
            try
            {
                string DeleteTurnUrl = $"/DeleteTurn/{Username}/{Password}/{TurnId}";
                Navigate.NavigateTo(DeleteTurnUrl);
            }

            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        public void RefreshPage(NavigationManager manager)
        {
            manager.NavigateTo(manager.Uri, true);
        }
    }
}
