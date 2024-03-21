using Blazored.Toast.Services;
using GiveTurn.Blazor.Services.Interfaces;
using GiveTurn.Model.Dtos;
using Microsoft.AspNetCore.Components;
using System.Security.AccessControl;

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
        public TurnDto UserLastTurn { get; set; }
        public AddTurnDto UserTurn { get; set; }
        public string ErrorMessage { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            User = await UserServices.Login(Username, Password);
            TurnDateTime = await TurnServices.GetTurnDateTime();
            UserMainPageUrl = $"/UserMainPage/{Username}/{Password}";
            UserLastTurn = await TurnServices.UserLastTurn(User.Id);
        }

        public async void SetTurn_Click()
        {
            if (!CheckTurn())
            {
                Toast.ShowError("you already have turn !");
                navigate.NavigateTo(UserMainPageUrl);
            }
            else
            {
                UserTurn = new AddTurnDto
                {
                    UserTurnDate = TurnDateTime,
                    Userid = User.Id,
                };

                var TunrResponse = await TurnServices.AddNewTurn(UserTurn);

                if (TunrResponse != null)
                {
                    User.HaveTurn = true;               // After add a PartUpdate methode for more optimize
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

        public bool CheckTurn()
        {
            try
            {
                if (User.HaveTurn)
                {
                    DateTime dtnow = DateTime.Now;
                    if (UserLastTurn.UserTurnDate < dtnow)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }

            catch
            {
                return false;
            }
        }

        public async void GetBack()
        {
            navigate.NavigateTo(UserMainPageUrl);
        }
    }
}
