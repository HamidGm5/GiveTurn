using Blazored.Toast.Services;
using GiveTurn.Blazor.Services.Interfaces;
using GiveTurn.Model.Dtos;
using Microsoft.AspNetCore.Components;

namespace GiveTurn.Web.Pages
{
    public class UpdateUserBase : ComponentBase
    {
        [Parameter]
        public string Username { get; set; }
        [Parameter]
        public string Password { get; set; }

        [Inject]
        public IUserServices UserServices { get; set; }
        [Inject]
        public IToastService Toast { get; set; }
        [Inject]
        public NavigationManager navigate { get; set; }

        public UserDto User { get; set; }
        public UserDto UpdateUser { get; set; }
        public string ChangeUsername { get; set; }
        public string ChangePassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string ChangePhoneNumber { get; set; }

        public string ErrorMessage { get; set; }
        protected override async Task OnParametersSetAsync()
        {
            try
            {
                User = await UserServices.Login(Username, Password);

                ChangeUsername = User.Username;
                ChangePassword = User.Password;
                ConfirmPassword = ChangePassword;
                ChangePhoneNumber = User.PhoneNumber;
            }

            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        public async void Update_Click()
        {
            try
            {
                if (User != null)
                {
                    if (ConfirmPassword == ChangePassword)
                    {
                        UpdateUser = new UserDto
                        {
                            Id = User.Id,
                            Username = ChangeUsername,
                            Password = ChangePassword,
                            PhoneNumber = ChangePhoneNumber
                        };

                        var UpdatedUser = await UserServices.UpdateUser(UpdateUser);
                        if (UpdatedUser != null)
                        {
                            Toast.ShowSuccess("Your account updated with success !");
                            string UserMainPageUl = $"/UserMainPage/{UpdatedUser.Username}/{UpdatedUser.Password}";
                            navigate.NavigateTo(UserMainPageUl);
                        }
                        else
                        {
                            Toast.ShowError("Something went wrong !");
                        }
                    }
                    else
                    {
                        Toast.ShowError("Password and Confirm Password Should be exactly match !");
                    }
                }
                else
                {
                    Toast.ShowError("Something went wrong !");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}
