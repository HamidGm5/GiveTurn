using Blazored.Toast.Services;
using GiveTurn.Blazor.Services.Interfaces;
using GiveTurn.Models.Dto;
using Microsoft.AspNetCore.Components;

namespace GiveTurn.Web.Pages
{
    public class GiveTurnBase : ComponentBase
    {
        [Parameter]
        public string UserName { get; set; }
        [Parameter]
        public string Password { get; set; }

        [Inject]
        public ITurnServices TurnServices { get; set; }
        [Inject]
        public IUserServices UserServices { get; set; }
        [Inject]
        public IToastService Toast { get; set; }

        public DateTime TurnDateTime { get; set; }
        public UserDto User { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            TurnDateTime = await TurnServices.GetTurnDateTime();
        }
    }
}
