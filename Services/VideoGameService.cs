using Microsoft.EntityFrameworkCore;
using VideoGameCharacterAPI.Data;
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

        public async Task<bool> DeleteCharacterAsync(int id)
        {
            var characterToDelete = await _context.Characters.FindAsync(id);
            if (characterToDelete is null)
                return false;

            _context.Characters.Remove(characterToDelete);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<PagedResponseDto<CharacterResponseDto>> GetCharactersAsync(GetCharactersQuery query)
        {
            var charactersQuery = _context.Characters.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Game))
            {
                charactersQuery = charactersQuery.Where(c => c.Game == query.Game);
            }

            if (!string.IsNullOrWhiteSpace(query.Role))
            {
                charactersQuery = charactersQuery.Where(c => c.Role == query.Role);
            }

            charactersQuery = (query.SortBy?.ToLower(), query.SortDirection?.ToLower()) switch
            {
                ("name", "desc") => charactersQuery.OrderByDescending(c => c.Name),
                ("name", _) => charactersQuery.OrderBy(c => c.Name),
                ("game", "desc") => charactersQuery.OrderByDescending(c => c.Game),
                ("game", _) => charactersQuery.OrderBy(c => c.Game),
                _ => charactersQuery.OrderBy(c => c.Id)
            };

            var totalCount = await charactersQuery.CountAsync();

            var page = query.Page < 1 ? 1 : query.Page;
            var pageSize = query.PageSize < 1 ? 10 : Math.Min(query.PageSize, 50);

            var items = await charactersQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CharacterResponseDto)
                {
                    Id = c.Id,
                    Name = c.Name,
                    Game = c.Game,
                    Role = c.Role
                }) 
                .ToListAsync();

            return new PagedResponseDto<CharacterResponseDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = items
            };
        }

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

        public async Task<bool> UpdateCharacterAsync(int id, UpdateCharacterRequest character)
        {
            var existingCharacter = await _context.Characters.FindAsync(id);
            if (existingCharacter is null)
                return false;

            existingCharacter.Name = character.Name;
            existingCharacter.Game = character.Game;
            existingCharacter.Role = character.Role;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}