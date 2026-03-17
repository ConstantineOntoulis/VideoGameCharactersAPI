using Microsoft.EntityFrameworkCore;
using VideoGameCharactersAPI.Data;
using VideoGameCharactersAPI.Models;
using VideoGameCharactersAPI.Dtos;

namespace VideoGameCharactersAPI.Services
{
    public class VideoGameService(CharacterDbContext _context) : IVideoGameCharacterService
    {
        public async Task<CharacterResponseDto> AddCharacterAsync(CreateCharacterRequest character)
        {
            var newCharacter = new Character
            {
                Name = character.Name,
                Game = character.Game,
                Role = character.Role
            };
            _context.Characters.Add(newCharacter);
            await _context.SaveChangesAsync();

            return new CharacterResponseDto
            {
                Id = newCharacter.Id,
                Name = newCharacter.Name,
                Game = newCharacter.Game,
                Role = newCharacter.Role
            };
        }

        public Task<bool> DeleteCharacterAsync(int id)
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