using Microsoft.EntityFrameworkCore;
using VideoGameCharactersAPI.Models;

namespace VideoGameCharacterAPI.Data
{
    public class CharacterDbContext(DbContextOptions<CharacterDbContext> options) : DbContext(options)
    {
        public DbSet<Character> Characters => Set<Character>();

    }
}
