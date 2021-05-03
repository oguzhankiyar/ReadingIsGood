using OK.ReadingIsGood.Shared.Core.Domain;

namespace OK.ReadingIsGood.Shared.Core.Requests
{
    public class BaseListRequest : ISortable, IPageable
    {
        public string Sort { get; set; }
        public string Order { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}