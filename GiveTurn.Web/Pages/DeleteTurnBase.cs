using Blazored.Toast.Services;
using GiveTurn.Blazor.Services.Interfaces;
using GiveTurn.Model.Dtos;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace GiveTurn.Web.Pages
{
    public class DeleteTurnBase : ComponentBase
    {
        [Parameter]
        public string Username { get; set; }
        [Parameter]
        public string Password { get; set; }
        [Parameter]
        public int TurnId { get; set; }

        [Inject]
        public IJSRuntime Js { get; set; }
        [Inject]
        public IToastService Toast { get; set; }
        [Inject]
        public IUserServices UserServices { get; set; }
        [Inject]
        public ITurnServices TurnServices { get; set; }
        [Inject]
        public NavigationManager Navigate { get; set; }

        public UserDto User { get; set; }
        public TurnDto Turn { get; set; }
        public DateTime UserTurnDateTime { get; set; }
        public string UserMainPageUrl { get; set; }
        public string ErrorMessage { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            try
            {
                User = await UserServices.Login(Username, Password);
                Turn = await TurnServices.GetUserTurn(User.Id, TurnId);
                UserMainPageUrl = $"/UserMainPage/{Username}/{Password}";
                UserTurnDateTime = Turn.UserTurnDate;
            }

            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        public async void DeleteTurn_Click()
        {
            try
            {
                var ConfirmPassword = await Js.InvokeAsync<string>("Prompting", "Enter your Password : ");
                if (ConfirmPassword == Password)
                {
                    bool DeleteTurn = await TurnServices.DeleteTurn(User.Id, TurnId);
                    if (DeleteTurn)
                    {
                        Toast.ShowSuccess("Your turn Deleted ! ");
                        Navigate.NavigateTo(UserMainPageUrl);
                        StateHasChanged();
                    }
                    else
                    {
                        Toast.ShowError("Somthing went wrong");
                    }
                }
                else
                {
                    Toast.ShowError("Your Password is wrong !");
                }
            }

            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}
