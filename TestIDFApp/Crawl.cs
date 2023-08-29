using System;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace TestIDFApp;

public class Crawl: ICrawl
{
    private IHttpClientFactory _httpClientFactory;

    public Crawl(IHttpClientFactory httpClientFactory) =>
        _httpClientFactory = httpClientFactory;

    public async Task<HttpResponseMessage> PerformHttpRequest(string uri, HttpMethod reqMethod)
    {
        
        var httpClient = _httpClientFactory.CreateClient();
        
        try
        {
            var httpRequestMessage = new HttpRequestMessage(reqMethod, uri);
            httpClient.DefaultRequestHeaders.Accept.ParseAdd("*/*");
            httpClient.DefaultRequestHeaders.AcceptEncoding.ParseAdd("gzip, deflate, br");
            httpClient.DefaultRequestHeaders.Connection.ParseAdd("keep-alive");
            return await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead);
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

    public async Task PerformPlaywrightScreenshotRequest(string uri)
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false,
        });
        var context = await browser.NewContextAsync();

        var page = await context.NewPageAsync();

        await page.GotoAsync(uri);

        await page.GetByRole(AriaRole.Button, new() { Name = "AGREE" }).ClickAsync();

        await page.ScreenshotAsync(new PageScreenshotOptions
        {
            FullPage = true,
            Path = "screenshot.png" 
        });

        await page.CloseAsync();
        await context.CloseAsync();
        await browser.CloseAsync();
    }
}