using VideoGameCharactersAPI.Models;

namespace VideoGameCharactersAPI.Services
{
    public class VideoGameService : IVideoGameCharacterService
    {
        static List<Character> characters = new List<Character>
        {
            new Models.Character { Id = 1, Name = "Noctis Lucis Caelum", Game = "Final Fantasy XV", Role = "Hero"},
            new Models.Character { Id = 2, Name = "Vincent Valentine", Game = "Dirge of Cerberus: Final Fantasy VII", Role = "Hero"},
            new Models.Character { Id = 3, Name = "G-Man", Game = "Half Life 3", Role = "Villain"},
            new Models.Character { Id = 4, Name = "Celeste", Game = "Deadlock", Role = "Hero"},
            new Models.Character { Id = 5, Name = "Yorha 2B", Game = "NieR: Automata", Role = "Hero"},
            new Models.Character { Id = 6, Name = "Soul of Cinder", Game = "Dark Souls III", Role = "Villain"},
            new Models.Character { Id = 7, Name = "Dick Grayson", Game = "Batman: Arkham Knight", Role = "Hero"},
            new Models.Character { Id = 8, Name = "Mr. X", Game = "Resident Evil 2", Role = "Villain"},
            new Models.Character { Id = 9, Name = "P", Game = "Lies of P", Role = "Hero"},
            new Models.Character { Id = 10, Name = "Genichiro Ashina", Game = "Sekiro: Shadows Die Twice", Role = "Villain"},
        };

        public Task<Character> AddCharacterAsync(Character character)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteCharacterAsynce(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Character>> GetAllCharactersAsync()
            => await Task.FromResult(characters);

        public async Task<Character?> GetCharacterByIdAsync(int id)
        {
            var result = characters.FirstOrDefault(c => c.Id == id);
            return await Task.FromResult(result);
        }

        public Task<bool> UpdateCharacterAsync(int id, Character character)
        {
            throw new NotImplementedException();
        }
    }
}