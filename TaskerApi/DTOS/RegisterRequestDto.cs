using System.ComponentModel.DataAnnotations;

namespace TaskerApi.DTOS
{
    public class RegisterRequestDto
    {
        [Required] public string Name { get; set; } = string.Empty;
        [Required][EmailAddress] public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(maximumLength: 64, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;
    }
}
