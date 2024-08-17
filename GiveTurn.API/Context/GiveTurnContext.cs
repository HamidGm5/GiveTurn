using GiveTurn.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace GiveTurn.API.Context
{
    public class GiveTurnContext : DbContext
    {
        public GiveTurnContext(DbContextOptions<GiveTurnContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Turn> Turns { get; set; }
        public DbSet<DeleteTurns> deleteTurns { get; set; }

    }
}
