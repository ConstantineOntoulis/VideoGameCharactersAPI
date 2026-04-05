using Microsoft.EntityFrameworkCore;
using VideoGameCharacterAPI.Data;
using VideoGameCharactersAPI.Dtos;
using VideoGameCharactersAPI.Models;
using VideoGameCharactersAPI.Services;

namespace VideoGameCharactersApi.Services
{
    //Implementation Class for character-related operations
    //Uses CharacterDbContext to read and later modify character data in SQL Server
    public class VideoGameService(CharacterDbContext _context, ILogger<VideoGameService> logger) : IVideoGameCharacterService
    {
        public async Task<CharacterResponseDto> AddCharacterAsync(CreateCharacterRequest character)
        {
            //Map the incoming request data to the database entity
            var newCharacter = new Character
            {
                Name = character.Name,
                Game = character.Game,
                Role = character.Role,
            };

            //Save the new character to the database
            _context.Characters.Add(newCharacter);
            await _context.SaveChangesAsync();

            //Log the successful creation event
            logger.LogInformation(
                "Character {CharacterId} was created for the game {Game} with the role {Role}.",
                newCharacter.Id,
                newCharacter.Game,
                newCharacter.Role
                );
            //Return the saved character as a response DTO
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
            var charToDelete = await _context.Characters.FindAsync(id);

            //Log a warning when the delete target does not exist
            if (charToDelete is null)
            {
                logger.LogWarning(
                    "Delete requested for the character {CharacterId}, but no matching character was found.",
                    id);
                return false;
            }

            _context.Characters.Remove(charToDelete);
            await _context.SaveChangesAsync();

            //Log the successful deletion event
            logger.LogInformation(
                "Character {CharacterId} was deleted successfully",
                id);
            return true;
        }
        public async Task<PagedResponseDto<CharacterResponseDto>> GetAllCharactersAsync(GetCharactersQuery query)
        {
            var charactersQuery = _context.Characters
                  .AsNoTracking() //Query is read-only, entity tracking is unnecessary, avoids tracking overhead
                  .AsQueryable(); //Convert source into a queryable pipeline

            if (!string.IsNullOrWhiteSpace(query.Game))
            { //Restrict results to characters whose Game value matches requested Game
                charactersQuery = charactersQuery.Where(c => c.Game == query.Game);
            }
            //When client has supplied non-empty role value, apply Role filter
            if (!string.IsNullOrWhiteSpace(query.Role))
            {//Restrict results to characters whose Role value matches requested Role
                charactersQuery = charactersQuery.Where(c => c.Role == query.Role);
            }

            //Sorting()
            //Apply sorting based on the requested sort field and direction. Convert values to lowercase for consistent comparisons
            charactersQuery = (query.SortBy?.ToLower(), query.SortDirection?.ToLower()) switch
            {
                ("name", "desc") => charactersQuery.OrderByDescending(c => c.Name), //Sort by Name in desc order
                ("name", _) => charactersQuery.OrderBy(c => c.Name), //Sort by Name in asc order when no valid descending direction is specified
                ("game", "desc") => charactersQuery.OrderByDescending(c => c.Game),
                ("game", _) => charactersQuery.OrderBy(c => c.Game),
                ("role", "desc") => charactersQuery.OrderByDescending(c => c.Role),
                ("role", _) => charactersQuery.OrderBy(c => c.Role),
                _ => charactersQuery.OrderBy(c => c.Id) //If no supported sort option is provided, fall back to Id ordering
            };

            //Pagination()
            //Normalize the requested page number. When client provides value smaller than 1, default to page 1
            var page = query.Page < 1 ? 1 : query.Page;
            //When client provides an invalid size, default to 10. When requested size is too large, cap it at 50.
            var pageSize = query.PageSize < 1 ? 10 : Math.Min(query.PageSize, 50);
            //Count the total number of rows that match the current filters before pagination is applied.
            var totalCount = await charactersQuery.CountAsync();
            //Retrieve only the rows that belong to the requested page
            var items = await charactersQuery
                .Skip((page - 1) * pageSize) //Skip all rows that belong to earlier pages, eg page 2, size 5 -> skip 5, page 3, size 5 -> skip 10
                .Take(pageSize) //Take only the number of rows needed for the current page
                .Select(c => new CharacterResponseDto //Project each entity into the response DTO shape
                {
                    Id = c.Id,
                    Name = c.Name,
                    Game = c.Game,
                    Role = c.Role
                })
               .ToListAsync(); //Execute the query and materialize the projected results as a list
            return new PagedResponseDto<CharacterResponseDto> //Return the paged response object, the paging metadata and the actual items for the current page
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = items
            };
        }

        //Returns a single character by id, or null if it does not exist
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
            { //Log a warning when the update target does not exist
                logger.LogWarning(
                    "Update requested for character {CharacterId}, but no matching character was found.",
                    id);
                return false;
            }

            existingCharacter.Name = character.Name;
            existingCharacter.Game = character.Game;
            existingCharacter.Role = character.Role;

            await _context.SaveChangesAsync();

            //Log the successful update event
            logger.LogInformation(
                "Character {CharacterId} was updated successfully.",
                id);
            return true;
        }
    }
}
