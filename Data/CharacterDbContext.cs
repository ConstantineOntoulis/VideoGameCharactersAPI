using Microsoft.EntityFrameworkCore;
using VideoGameCharactersAPI.Models;

namespace VideoGameCharacterAPI.Data
{
    //Database context used by EF Core to query and persist application data.
    public class CharacterDbContext(DbContextOptions<CharacterDbContext> options) : DbContext(options)
    {
        //Provides access to Character entities stored in the database.
        public DbSet<Character> Characters => Set<Character>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configure database-specific rules for the Character entity
            modelBuilder.Entity<Character>()
                .HasIndex(c => c.Game);
            base.OnModelCreating(modelBuilder); //Preserve the base DbContext model configuration behavior
        }
    }
}