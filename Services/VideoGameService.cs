using Microsoft.EntityFrameworkCore;
using VideoGameCharactersAPI.Data;
using VideoGameCharactersAPI.Models;
using VideoGameCharactersAPI.Dtos;

namespace VideoGameCharactersAPI.Services
{
    public class VideoGameService(CharacterDbContext _context) : IVideoGameCharacterService
    {
        public Task<CharacterResponseDto> AddCharacterAsync(CreateCharacterRequest character)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteCharacterAsynce(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CharacterResponseDto>> GetAllCharactersAsync()
            => await _context.Characters.Select(c => new CharacterResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                Game = c.Game,
                Role = c.Role
            }).ToListAsync();

        public async Task<CharacterResponseDto?> GetCharacterByIdAsync(int id)
        {
            var result = await _context.Characters
                .Where(c => c.Id == id)
                .Select(c => new CharacterResponseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Game = c.Game,
                    Role = c.Role
                })
                .FirstOrDefaultAsync();

            return result;
        }

        public Task<bool> UpdateCharacterAsync(int id, UpdateCharacterRequest character)
        {
            throw new NotImplementedException();
        }
    }
}