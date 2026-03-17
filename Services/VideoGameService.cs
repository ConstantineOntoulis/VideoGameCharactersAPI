using Microsoft.EntityFrameworkCore;
using VideoGameCharactersAPI.Data;
using VideoGameCharactersAPI.Models;

namespace VideoGameCharactersAPI.Services
{
    public class VideoGameService(CharacterDbContext _context) : IVideoGameCharacterService
    {
        public Task<Character> AddCharacterAsync(Character character)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteCharacterAsynce(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Character>> GetAllCharactersAsync()
            => await _context.Characters.ToListAsync();

        public async Task<Character?> GetCharacterByIdAsync(int id)
        {
            var result = await _context.Characters.FindAsync(id);
            return result;
        }

        public Task<bool> UpdateCharacterAsync(int id, Character character)
        {
            throw new NotImplementedException();
        }
    }
}