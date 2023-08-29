using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TestIDFApp.Models;

namespace TestIDFApp.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class IdfController : ControllerBase
{
    private readonly ILogger<IdfController> _logger;
    private readonly ICrawl _crawl;

    public IdfController(ILogger<IdfController> logger, ICrawl crawl)
    {
        _logger = logger;
        _crawl = crawl;
    }

    [HttpPost]
    public async Task<ActionResult<string>> Screenshot([FromBody] IdfModel idfModel)
    {
        var url = idfModel.ResourceUrl;

        await _crawl.PerformPlaywrightScreenshotRequest(url);
        
        return Ok("Screenshot: "+ url);
    }
    
    [HttpPost]
    public async Task<ActionResult<string>> Check([FromBody] IdfModel idfModel)
    {
        var url = idfModel.ResourceUrl;

        var response = await _crawl.PerformHttpRequest(url, HttpMethod.Get);

        _logger.LogDebug(response.Content.ToString());
        
        return Ok(response.StatusCode);

    }
}