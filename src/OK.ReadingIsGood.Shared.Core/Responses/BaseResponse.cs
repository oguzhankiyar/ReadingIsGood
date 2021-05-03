namespace OK.ReadingIsGood.Shared.Core.Responses
{
    public class BaseResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public string[] Errors { get; set; }

        public BaseResponse()
        {
            Status = true;
            Message = "Success";
            Errors = default;
        }
    }
}