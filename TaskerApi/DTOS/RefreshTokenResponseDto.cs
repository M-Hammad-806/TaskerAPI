namespace TaskerApi.DTOS
{
    public class RefreshTokenResponseDto
    {
        public int UserId { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }

}
