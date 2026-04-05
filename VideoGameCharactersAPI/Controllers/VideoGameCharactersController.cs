using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VideoGameCharactersAPI.Dtos;
using VideoGameCharactersAPI.Services;

namespace VideoGameCharactersApi.Controllers;

[Route("api/[controller]")]
[ApiController]
//Thin controller: handles the HTTP request and delegates data retrieval to the service layer.
public class VideoGameCharactersController(IVideoGameCharacterService service) : ControllerBase
{
    [Authorize(Policy = "UserOrAdmin")]
    [HttpGet]
    //Returns all characters through the service abstraction rather than accessing data directly.
    public async Task<ActionResult<PagedResponseDto<CharacterResponseDto>>> GetCharacters([FromQuery] GetCharactersQuery query)
    {
        //throw new InvalidOperationException("Deliberate test exception.");
        return Ok(await service.GetAllCharactersAsync(query));
    }

    //Returns a single character by id, or 404 if no matching character exists.
    [Authorize(Policy = "UserOrAdmin")]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CharacterResponseDto>> GetCharacter(int id)
    {
        var character = await service.GetCharacterByIdAsync(id);
        if (character is null)
        {
            return Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "Character not found.",
                detail: $"No character with id {id} was found.",
                instance: HttpContext.Request.Path,
                type: "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.5");
        }
        return Ok(character); //200 OK
    }

    //The return type is partly about data, and partly about status meaning
    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    public async Task<ActionResult<CharacterResponseDto>> AddCharacter(CreateCharacterRequest character)
    {
        var createdCharacter = await service.AddCharacterAsync(character);
        return CreatedAtAction(nameof(GetCharacter), new { id = createdCharacter.Id }, createdCharacter); //201 Created, can be retrieved through GetCharacter using this id
    }

    [Authorize(Policy = "AdminOnly")]
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
        return NoContent(); //204 No Content, request succeeded, no response body is necessary
    }
    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteCharacter(int id)
    {
        var deleted = await service.DeleteCharacterAsync(id);
        if (!deleted)
        {
            return Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "Character not found.",
                detail: $"No character with id {id} was found.",
                instance: HttpContext.Request.Path,
                type: "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.5");
        }
        return NoContent(); //204 No Content, request succeeded, no response body is necessary
    }
}
