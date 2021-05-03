namespace OK.ReadingIsGood.Identity.Contracts.Models
{
    public class AuthTokenModel
    {
        public string AccessToken { get; set; }
        public double ExpiresIn { get; set; }
    }
}