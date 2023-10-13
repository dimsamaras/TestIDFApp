namespace TestIDFApp;

public interface ICrawlHttpClient
{
    public Task<HttpResponseMessage> PerformHttpRequest(string uri, HttpMethod reqMethod);
}