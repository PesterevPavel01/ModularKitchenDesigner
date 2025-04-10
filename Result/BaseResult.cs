namespace Result
{
    public class BaseResult 
    {
        public bool IsSuccess => ErrorMessage == null;
        public string? ErrorMessage { get; set; }
        public int? ErrorCode { get; set; }
        public string? ObjectName { get; set; }
        public DateTime? ConnectionTime { get; set; }

    }

    public class BaseResult<TItem> : BaseResult, ISuccessubleResult<TItem>
    {
        public BaseResult(string errorMessage, int errorCode, TItem data, string objectName)
        {
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
            Data = data;
            ObjectName = objectName;
        }

        public TItem Data { get; set; }

        public BaseResult() { }
    }
}
