using System.Net.Sockets;

namespace TestIDFApp;

public class CrawlHttpClient : ICrawlHttpClient
{
    private readonly HttpClient _serviceHttpClient;

    public CrawlHttpClient(HttpClient serviceHttpClient)
    {
        _serviceHttpClient = serviceHttpClient;
    }

    public async Task<HttpResponseMessage> PerformHttpRequest(string uri, HttpMethod reqMethod)
    {
        
        try
        {
            var httpRequestMessage = new HttpRequestMessage(reqMethod, uri);
            _serviceHttpClient.DefaultRequestHeaders.Accept.ParseAdd("*/*");
            _serviceHttpClient.DefaultRequestHeaders.AcceptEncoding.ParseAdd("gzip, deflate, br");
            _serviceHttpClient.DefaultRequestHeaders.Connection.ParseAdd("keep-alive");
            return await _serviceHttpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead);
        }
        catch (Exception ex) when (ex.InnerException != null &&
                                   ex.InnerException.GetType() == typeof(TimeoutException))
        {
            throw new TimeoutException("Request timeout", ex);
        }
        catch (Exception ex) when (ex.InnerException != null &&
                                   ex.InnerException.GetType() == typeof(SocketException) &&
                                   ((SocketException) ex.InnerException).SocketErrorCode ==
                                   SocketError.HostNotFound)
        {
            throw new Exception("Host not Found.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("Request failed.", ex);
        }
    }
}