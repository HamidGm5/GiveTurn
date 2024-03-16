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
        public IToastService Toast { get; set; }

        public UserDto User { get; set; }
        public string ErrorMessage { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            try
            {
                User = await UserServices.Login(Username , Password);
            }

            catch (Exception ex) 
            {
                ErrorMessage = ex.Message;
            }
        }

        public async void Update_Click()
        {

        }

        public async void Delete_Click()
        {

        }
    }
}
