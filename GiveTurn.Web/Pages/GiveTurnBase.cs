using Blazored.Toast.Services;
using GiveTurn.Blazor.Services.Interfaces;
using GiveTurn.Models.Dto;
using Microsoft.AspNetCore.Components;

namespace GiveTurn.Web.Pages
{
    public class GiveTurnBase : ComponentBase
    {
        [Parameter]
        public string Username { get; set; }
        [Parameter]
        public string Password { get; set; }

        [Inject]
        public ITurnServices TurnServices { get; set; }
        [Inject]
        public IUserServices UserServices { get; set; }
        [Inject]
        public IToastService Toast { get; set; }
        [Inject]
        public NavigationManager navigate { get; set; }

        public string UserMainPageUrl { get; set; }
        public DateTime TurnDateTime { get; set; }
        public UserDto User { get; set; }
        public AddTurnDto UserTurn { get; set; }
        public string ErrorMessage { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            User = await UserServices.Login(Username, Password);
            TurnDateTime = await TurnServices.GetTurnDateTime();
            UserMainPageUrl = $"/UserMainPage/{Username}/{Password}";
        }

        public async void SetTurn_Click()
        {
            UserTurn = new AddTurnDto
            {
                UserTurnDate = TurnDateTime,
                Userid = User.Id,
            };

            if (User.HaveTurn)
            {
                ErrorMessage = "you already have turn !";
            }
            else
            {
                var TunrResponse = await TurnServices.AddNewTurn(UserTurn);

                if (TunrResponse != null)
                {
                    User.HaveTurn = true;
                    await UserServices.UpdateUser(User);
                    navigate.NavigateTo(UserMainPageUrl);
                    Toast.ShowSuccess("You take a success turn !");
                }
                else
                {
                    Toast.ShowError("something went wrong , check again");
                }
            }
        }

        public async void GetBack()
        {
            navigate.NavigateTo(UserMainPageUrl);
        }
    }
}
