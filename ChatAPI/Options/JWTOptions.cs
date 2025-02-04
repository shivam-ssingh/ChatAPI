namespace ChatAPI.Options
{
    public class JWTOptions
    {
        public const string JWTOption = "JWT";
        public string Secret { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string ExpirationInMinutes { get; set; } = string.Empty;
    }
}
