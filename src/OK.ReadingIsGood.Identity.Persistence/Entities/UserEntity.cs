using OK.ReadingIsGood.Shared.Persistence.Base;

namespace OK.ReadingIsGood.Identity.Persistence.Entities
{
    public class UserEntity : EntityBase
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}