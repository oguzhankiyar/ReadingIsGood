namespace OK.ReadingIsGood.Identity.Business.Config
{
    public class IdentityBusinessConfig
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public double ExpirationMinutes { get; set; }
        public string SecurityKey { get; set; }
    }
}