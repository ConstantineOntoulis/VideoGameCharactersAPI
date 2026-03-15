using Microsoft.EntityFrameworkCore;

namespace VideoGameCharactersAPI.Data
{
    public class CharacterDbContext(DbContextOptions<CharacterDbContext> options) : DbContext
    {

    }
}
