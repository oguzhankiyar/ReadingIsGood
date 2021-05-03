namespace OK.ReadingIsGood.Shared.Core.Events.User
{
    public class UserCreatedEvent
    {
        public class UserModel
        {
            public int Id { get; set; }
            public string Email { get; set; }
            public string FullName { get; set; }
        }

        public UserModel User { get; set; }
    }
}