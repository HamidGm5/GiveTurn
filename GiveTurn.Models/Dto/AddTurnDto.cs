using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiveTurn.Models.Dto
{
    public class AddTurnDto
    {
        public int Id { get; set; }
        public DateTime UserTurnDate { get; set; }

        public int Userid { get; set; }
    }
}
