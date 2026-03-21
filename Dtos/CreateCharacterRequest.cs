using System.ComponentModel.DataAnnotations;

namespace VideoGameCharactersAPI.Dtos
{
    public class CreateCharacterRequest
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Game is required.")]
        [StringLength(100, ErrorMessage = "Game cannot exceed 100 characters.")]
        public string Game { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role is required.")]
        [RegularExpression(
            "^(protagonist|hero|antagonist|villain)$",
            ErrorMessage = "Role must be one of: protagonist, hero, antagonist, villain.")]
        public string Role { get; set; } = string.Empty;
    }
}
