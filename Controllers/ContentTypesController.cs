using Microsoft.AspNetCore.Mvc;
using Graphitie.Services;

namespace Graphitie.Controllers.Microsoft;

[ApiController]
[Route("[controller]")]
public class ContentTypesController(ILogger<ContentTypesController> logger, MicrosoftService microsoftService) : ControllerBase
{
    private readonly ILogger<ContentTypesController> _logger = logger;

    private readonly MicrosoftService _microsoftService = microsoftService;

    [HttpPost("Add", Name = "AddContentType")]
    public async Task<IActionResult> AddContentType(string siteUrl, string name, string listTitle)
    {
        await _microsoftService.AddContentType(siteUrl, name, listTitle);

        return new OkResult();

    }

    [HttpGet("Get", Name = "GetContentTypes")]
    public async Task<IActionResult> GetContentTypes(string siteUrl)
    {
        var result = await _microsoftService.GetAllContentTypes(siteUrl);

        return new ObjectResult(result);

    }

    [HttpPost("Default", Name = "SetDefaultContentType")]
    public async Task<IActionResult> SetDefaultContentType(string siteUrl, string name, string listTitle)
    {
        await _microsoftService.SetDefaultContentType(siteUrl, name, listTitle);

        return new OkResult();
    }

    [HttpDelete("Delete", Name = "DeleteContentType")]
    public IActionResult DeleteContentType(string siteUrl, string name, string listTitle, bool ignoreInUseException = false)
    {
        _microsoftService.DeleteContentType(siteUrl, name, listTitle, ignoreInUseException);

        return new OkResult();
    }
}
