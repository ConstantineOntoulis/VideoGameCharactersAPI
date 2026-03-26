using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VideoGameCharactersAPI.Models;
using VideoGameCharactersAPI.Services;
using VideoGameCharactersAPI.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace VideoGameCharactersAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VideoGameCharactersController(IVideoGameCharacterService service) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<CharacterResponseDto>>> GetCharacters()
        {
            return Ok(await service.GetAllCharactersAsync());
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CharacterResponseDto>> GetCharacter(int id)
        {
            var character = await service.GetCharacterByIdAsync(id);
            if(character is null)
            {
                return Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Character not found.",
                    detail:$"No character with id {id} was found.",
                    instance: HttpContext.Request.Path,
                    type: "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.5");
            }
            return Ok(character);
        }

        [HttpPost]
        public async Task<ActionResult<CharacterResponseDto>> AddCharacter(CreateCharacterRequest character)
        {
            var createdCharacter = await service.AddCharacterAsync(character);
            return CreatedAtAction(nameof(GetCharacter), new { id = createdCharacter.Id }, createdCharacter);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateCharacter(int id, UpdateCharacterRequest character)
        {
            var updated = await service.UpdateCharacterAsync(id, character);
            if (!updated)
            {
                return Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Character not found.",
                    detail: $"No character with id {id} was found.",
                    instance: HttpContext.Request.Path,
                    type: "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.5");
            }

            return NoContent();
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteCharacter(int id)
        {
            var deleted = await service.DeleteCharacterAsync(id);
            return deleted ? NoContent() : NotFound("Character with the given Id was not found.");
        }

    }
}
