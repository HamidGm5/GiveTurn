namespace GiveTurn.API.Entities
{
    public class Turn
    {
        public int Id { get; set; }
        public DateTime UserTurnDate { get; set; }

        public User User { get; set; }
    }
}
