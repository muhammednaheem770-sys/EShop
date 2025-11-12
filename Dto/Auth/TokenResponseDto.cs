namespace EShop.Dto.Auth
{
    public class TokenResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public string Token {  get; set; } = string.Empty;
        public DateTime? RefreshTokenExpiryTime { get; set; }

        public TokenResponseDto(string token, string refreshToken)
        {
            Token = token;
            RefreshToken = refreshToken;
        }

        public TokenResponseDto(string token, string refreshToken, DateTime expiryTime)
        {
            Token = token;
            RefreshToken = refreshToken;
        }

        public TokenResponseDto(string token, string refreshToken, DateTime? expiry)
        : this(token, refreshToken)
        {
            RefreshTokenExpiryTime = expiry;
        }

        public TokenResponseDto() { }
    }
}
