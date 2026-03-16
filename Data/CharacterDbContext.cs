using Microsoft.EntityFrameworkCore;
using VideoGameCharactersAPI.Models;

namespace VideoGameCharactersAPI.Data
{
    public class CharacterDbContext(DbContextOptions<CharacterDbContext> options) : DbContext(options)
    {
        public DbSet<Character> Characters => Set<Character>();

    }
}
