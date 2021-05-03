namespace OK.ReadingIsGood.Shared.Persistence.Domain
{
    public interface IIdentifiable<T>
    {
        T Id { get; set; }
    }
}
