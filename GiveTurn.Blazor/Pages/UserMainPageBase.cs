using Microsoft.AspNetCore.Components;

namespace GiveTurn.Blazor.Pages
{
    public class UserMainPageBase : ComponentBase
    {
        [Parameter]
        public string Username { get; set; }
        [Parameter]
        public string Password { get; set; }
    }
}
