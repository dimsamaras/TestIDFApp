namespace TestIDFApp;

public interface ICrawl
{
    public Task<HttpResponseMessage> PerformHttpRequest(string uri, HttpMethod reqMethod);
    
    public Task PerformPlaywrightScreenshotRequest(string uri);    
}