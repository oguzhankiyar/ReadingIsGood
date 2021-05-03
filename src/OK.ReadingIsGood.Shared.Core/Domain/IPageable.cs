namespace OK.ReadingIsGood.Shared.Core.Domain
{
    public interface IPageable
    {
        int PageNumber { get; set; }
        int PageSize { get; set; }
    }
}