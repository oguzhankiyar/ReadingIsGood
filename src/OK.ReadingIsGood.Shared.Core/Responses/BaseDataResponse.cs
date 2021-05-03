namespace OK.ReadingIsGood.Shared.Core.Responses
{
    public class BaseDataResponse<T> : BaseResponse
    {
        public T Data { get; set; }

        public BaseDataResponse()
        {
            Data = default;
        }
    }
}