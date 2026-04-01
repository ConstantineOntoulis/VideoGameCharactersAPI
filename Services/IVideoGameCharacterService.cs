using VideoGameCharactersAPI.Dtos;

namespace VideoGameCharactersAPI.Services
{
    public interface IVideoGameCharacterService
    {
        // Add CRUD Methods within the interface
        //Return DTOs instead of entity models in API responses
        Task<PagedResponseDto<CharacterResponseDto>> GetAllCharactersAsync(GetCharactersQuery query); //get a queried page of characters based on filters/sort/pagination
        Task<CharacterResponseDto?> GetCharacterByIdAsync(int id);
        Task<CharacterResponseDto> AddCharacterAsync(CreateCharacterRequest character);
        Task<bool> UpdateCharacterAsync(int id, UpdateCharacterRequest character);
        Task<bool> DeleteCharacterAsync(int id);
    }
}