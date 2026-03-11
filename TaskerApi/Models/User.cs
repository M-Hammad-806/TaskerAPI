namespace TaskerApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string HashPassword { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public string? RefreshToken { get; set; } 
        public DateTime RefreshTokenExpirationDate { get; set; }

    }
}

