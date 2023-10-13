using Microsoft.Playwright;

namespace TestIDFApp;

public class Crawl: ICrawl
{
    private readonly ICrawlHttpClient _serviceHttpClient;

    public Crawl(ICrawlHttpClient serviceHttpClient) =>
        _serviceHttpClient = serviceHttpClient;


    public Task<HttpResponseMessage> PerformHttpRequest(string uri, HttpMethod reqMethod)
    {
        return _serviceHttpClient.PerformHttpRequest(uri, reqMethod);
    }

    public async Task PerformPlaywrightScreenshotRequest(string uri)
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false,
            Proxy = new Proxy { Server = "per-context" }
        });

        var context = await browser.NewContextAsync(new BrowserNewContextOptions
                {
                    Proxy = new Proxy {Server = Helper.SelectProxy()}
                }
            );

        var page = await context.NewPageAsync();

        await page.GotoAsync(uri);
        
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