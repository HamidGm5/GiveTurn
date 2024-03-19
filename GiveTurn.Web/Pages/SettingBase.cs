using Blazored.Toast.Services;
using GiveTurn.Blazor.Services.Interfaces;
using GiveTurn.Models.Dto;
using Microsoft.AspNetCore.Components;

namespace GiveTurn.Web.Pages
{
    public class SettingBase : ComponentBase
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
        public NavigationManager Navigate { get; set; }

        public UserDto User { get; set; }
        public string ErrorMessage { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            try
            {
                User = await UserServices.Login(Username, Password);
            }

            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        public async void Update_Click()
        {
            string UpdateUserUrl = $"/UpdateUser/{Username}/{Password}";
            Navigate.NavigateTo(UpdateUserUrl);
        }

        public async void DeleteTurns_Click()
        {
            var DeleteTurns = await TurnServices.DeleteAllUserTurns(User.Id);
            if (DeleteTurns != null)
            {
                Toast.ShowSuccess("Your Turns deleted successful");
                Navigate.NavigateTo($"/UserMainPage/{Username}/{Password}");
            }
            else
            {
                Toast.ShowError("Something went wrong !");
            }
        }

        public async void DeleteAccount_Click()
        {
            string DeleteUserUrl = $"/DeleteUser/{Username}/{Password}";
            Navigate.NavigateTo(DeleteUserUrl);
        }
    }
}
