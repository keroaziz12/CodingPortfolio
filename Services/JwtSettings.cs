namespace CodingPortfolio.Services
{
    public class JwtSettings
    { 
        public const string SectionName = "jwt";
        public string key { get; set; } = string.Empty;

        public string Issuer { get; set; } = string.Empty;

        public string Audience { get; set; } = string.Empty;

        public int ExpiresInMinutes { get; set; } 
    }
}
