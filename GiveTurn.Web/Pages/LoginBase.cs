using Blazored.Toast.Services;
using GiveTurn.Blazor.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace GiveTurn.Web.Pages
{
    public class LoginBase : ComponentBase
    {

        public string Username { get; set; }
        public string Password { get; set; }

        [Inject]
        public IUserServices UserServices { get; set; }

        [Inject]
        public NavigationManager navigation { get; set; }

        [Inject]
        public IToastService Toast { get; set; }

        public string UserMainPageURL { get; set; }
        public string ErrorMessage { get; set; }


        public async void Login_Click()
        {
            try
            {
                if (Username == null || Password == null)
                {
                    Toast.ShowWarning("Enter Your Username and Password");
                }
                else
                {
                    var User = await UserServices.Login(Username, Password);

                    if (User == null)
                    {
                        Toast.ShowError("Please Enter a correct username and password");
                    }
                    else
                    {
                        Toast.ShowSuccess("You are Login With Sucess");
                        UserMainPageURL = $"/UserMainPage/{Username}/{Password}";
                        navigation.NavigateTo(UserMainPageURL);
                    }
                }
            }

            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                Toast.ShowError(ErrorMessage);
            }
        }
    }
}
