namespace OK.ReadingIsGood.Shared.Core.Responses
{
    public class BasePagedResponse<T> : BaseListResponse<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int TotalCount { get; set; }

        public BasePagedResponse()
        {

        }
    }
}