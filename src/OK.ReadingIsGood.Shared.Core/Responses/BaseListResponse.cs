using System;

namespace OK.ReadingIsGood.Shared.Core.Responses
{
    public class BaseListResponse<T> : BaseDataResponse<T[]>
    {
        public BaseListResponse()
        {
            Data = Array.Empty<T>();
        }
    }
}