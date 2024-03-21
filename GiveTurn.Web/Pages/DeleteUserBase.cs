using Blazored.Toast.Services;
using GiveTurn.Blazor.Services.Interfaces;
using GiveTurn.Model.Dtos;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace GiveTurn.Web.Pages
{
    public class DeleteUserBase : ComponentBase
    {
        [Parameter]
        public string Username { get; set; }
        [Parameter]
        public string Password { get; set; }
        [Inject]
        public IUserServices UserServices { get; set; }
        [Inject]
        public IJSRuntime Js { get; set; }
        [Inject]
        public IToastService Toast { get; set; }
        [Inject]
        public NavigationManager Navigate { get; set; }
        public UserDto User { get; set; }
        public string ConfirmPassword { get; set; }
        public string ErrorMessage { get; set; }


        protected override async Task OnParametersSetAsync()
        {
            try
            {
                User = await UserServices.Login(Username, Password);
                if (User == null)
                {
                    Toast.ShowError("Something went wrong try again !");
                }
            }

            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        public async void Prompt_Click()
        {
            ConfirmPassword = await Js.InvokeAsync<string>("Prompting", "Enter Your Password :");
            if (User != null)
            {
                if (ConfirmPassword == User.Password)
                {
                    bool deleteUser = await UserServices.DeleteUser(User.Id);
                    if (deleteUser)
                    {
                        Toast.ShowInfo("Your Account was successfuly Deleted");
                        Navigate.NavigateTo("/");
                    }
                    else
                    {
                        Toast.ShowError("Something Went Wrong");
                        Navigate.NavigateTo("/");
                    }
                }
                else
                {
                    Toast.ShowError("Your password is incorrect");
                }
            }
            else
            {
                ErrorMessage = "You are delete your account in past !";
            }
        }
    }
}
