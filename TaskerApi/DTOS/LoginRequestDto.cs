using System.ComponentModel.DataAnnotations;

namespace TaskerApi.DTOS
{
    public class LoginRequestDto
    {
        [EmailAddress] public string Email { get; set; } = string.Empty;

        [StringLength(maximumLength:64,MinimumLength =8)][Required]public string Password { get; set; } = string.Empty;
    }

}
