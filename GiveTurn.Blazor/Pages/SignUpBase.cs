using Blazored.Toast.Services;
using GiveTurn.Blazor.Services.Interfaces;
using GiveTurn.Models.Dto;
using Microsoft.AspNetCore.Components;

namespace GiveTurn.Blazor.Pages
{
    public class SignUpBase : ComponentBase
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public UserDto NewUser { get; set; }

        [Inject]
        public IUserServices UserServices { get; set; }

        [Inject]
        public IToastService Toast { get; set; }

        [Inject]
        public NavigationManager Navigation { get; set; }

        public string UserMainPageUrl { get; set; }

        public async void SignUpBtn_Click()
        {
            if (Username == null || Password == null || PhoneNumber == null)
            {
                Toast.ShowWarning("Please Fill all Field");
            }
            else
            {
                if (IsDigit(PhoneNumber).Contains(false))
                {
                    Toast.ShowWarning("Phone Number should be a correct !");

                }
                else
                {
                    NewUser = new UserDto()
                    {
                        Username = Username,
                        Password = Password,
                        PhoneNumber = PhoneNumber,
                        HaveTurn = false
                    };

                    var AddUser = await UserServices.SignUp(NewUser);

                    if (AddUser != null)
                    {
                        Toast.ShowSuccess("You are SignUp With Success !");
                        UserMainPageUrl = $"/UserMainPage/{Username}/{Password}";
                        Navigation.NavigateTo(UserMainPageUrl);
                    }
                    else
                    {
                        Toast.ShowError("Your Username befor taken !");
                    }
                }
            }
        }

        public List<bool> IsDigit(string PhoneNumberDigit)
        {
            List<bool> result = new List<bool>();
            foreach (char digit in PhoneNumberDigit)
            {
                if (!Char.IsDigit(digit))
                {
                    result.Add(false);
                }
                else
                {
                    result.Add(true);
                }
            }
            return result;
        }
    } // Class
}
