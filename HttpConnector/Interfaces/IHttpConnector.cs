using Result;

namespace HttpConnector.Interfaces
{
    public interface IHttpConnector
    {
        public Task<BaseResult<string>> GetDataByUrlAsync(string url, string bodyData, HttpMethod method);
    }
}
