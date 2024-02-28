namespace GiveTurn.API.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public bool HaveTurn { get; set; } = false;

        public ICollection<Turn> Turn { get; set; }
    }
}
