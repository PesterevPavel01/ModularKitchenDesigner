using HttpConnector.Interfaces;
using Newtonsoft.Json;
using Resources;
using Result;
using System.Net;
using System.Text;

namespace HttpConnector.Services
{
    public class HttpConnector : IHttpConnector
    {
        public async Task<BaseResult<string>> GetDataByUrlAsync(string url, string bodyData, HttpMethod method)
        {
            BaseResult<string> result = new();

            WebRequest request = WebRequest.Create(url);

            request.ContentType = "application/json";

            request.Method = method.Method;

            BaseResult<string> requestResult = new();

            if (!string.IsNullOrEmpty(bodyData))
            {
                using var streamWriter = new StreamWriter(await request.GetRequestStreamAsync(), Encoding.UTF8);
                await streamWriter.WriteAsync(bodyData);
            }

            try
            {
                using HttpWebResponse httpResponse = (HttpWebResponse)await request.GetResponseAsync();
                using StreamReader reader = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8);
                string responseStr = await reader.ReadToEndAsync();
                requestResult.Data = responseStr;
            }
            catch (Exception ex)
            {
                requestResult.ErrorMessage = ex.Message;
                requestResult.ObjectName = "HttpConnector";
                return requestResult;
            }


            if (requestResult.Data is not null)
            {
                try 
                {
                    result = JsonConvert.DeserializeObject<BaseResult<string>>(requestResult.Data);
                }
                catch (Exception ex)
                {
                    return new()
                    {
                        ErrorMessage = ex.Message,
                        ObjectName = "HttpConnector",
                    };
                }
                result.ObjectName = "HttpConnector";
                return result;
            }
            else
                return new()
                {
                    ErrorMessage = ErrorMessages.InternalServerError,
                    ObjectName = "HttpConnector",
                };
        }
    }
}
