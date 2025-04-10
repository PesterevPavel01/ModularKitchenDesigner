namespace Result
{
    public interface ISuccessubleResult<TItem>
    {
        bool IsSuccess { get; }
        string ErrorMessage { get; set; }
        int? ErrorCode { get; set; }
        string ObjectName { get; set; }
    }
}
