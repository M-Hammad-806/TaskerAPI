namespace TaskerApi.DTOS
{
    public class RegisterResponseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
    }
}
