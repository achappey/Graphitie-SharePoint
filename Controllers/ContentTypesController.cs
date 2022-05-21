using Microsoft.AspNetCore.Mvc;
using Graphitie.Services;
using Microsoft.AspNetCore.Authorization;

namespace Graphitie.Controllers.Microsoft;

[ApiController]
[Route("[controller]")]
//[Authorize]
public class ContentTypesController : ControllerBase
{
    private readonly ILogger<ContentTypesController> _logger;

    private readonly MicrosoftService _microsoftService;

    public ContentTypesController(ILogger<ContentTypesController> logger, MicrosoftService microsoftService)
    {
        _logger = logger;
        _microsoftService = microsoftService;
    }
    
    [HttpPost("Add", Name = "AddContentType")]
    public async Task AddContentType(string siteUrl, string name, string listTitle)
    {
        await _microsoftService.AddContentType(siteUrl, name, listTitle);

    }

    [HttpPost("Default", Name = "SetDefaultContentType")]
    public async Task SetDefaultContentType(string siteUrl, string name, string listTitle)
    {
        await _microsoftService.SetDefaultContentType(siteUrl, name, listTitle);

    }

    [HttpDelete("Delete", Name = "DeleteContentType")]
    public IActionResult DeleteContentType(string siteUrl, string name, string listTitle, bool ignoreInUseException = false)
    {
        _microsoftService.DeleteContentType(siteUrl, name, listTitle, ignoreInUseException);

        return new OkResult();
    }
}
