using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiveTurn.Model.Dtos
{
    public class DeleteTurnsDto
    {
        public int ID { get; set; }
        public int Userid { get; set; }
        public DateTime TurnDate { get; set; }

    }
}
