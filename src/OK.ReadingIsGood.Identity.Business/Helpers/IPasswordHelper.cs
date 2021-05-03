namespace OK.ReadingIsGood.Identity.Business.Helpers
{
    public interface IPasswordHelper
    {
        string Hash(string password);
        bool Verify(string password, string hash);
    }
}