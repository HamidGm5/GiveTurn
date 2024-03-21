using GiveTurn.Model.Dtos;
using Microsoft.AspNetCore.Components;

namespace GiveTurn.Web.Pages
{
    public class TurnDisplayBase : ComponentBase
    {
        [Parameter]
        public ICollection<TurnDto> UserTurns { get; set; }
    }
}
