using VideoGameCharactersAPI.Models;
using VideoGameCharactersAPI.Dtos;

namespace VideoGameCharactersAPI.Services
{
    public interface IVideoGameCharacterService
    {
        //Add CRUD Methods within the interface
        Task<List<CharacterResponseDto>> GetAllCharactersAsync();
        Task<CharacterResponseDto?> GetCharacterByIdAsync(int id);
        Task<CharacterResponseDto> AddCharacterAsync(CreateCharacterRequest character);
        Task<bool> UpdateCharacterAsync(int id, UpdateCharacterRequest character);
        Task<bool> DeleteCharacterAsynce(int id);
    }
}