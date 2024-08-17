namespace GiveTurn.API.Entities
{
    public class DeleteTurns
    {
        public int ID { get; set; }
        public int Userid { get; set; }
        public DateTime TurnDate { get; set; }

        public User user { get; set; }
    }
}
