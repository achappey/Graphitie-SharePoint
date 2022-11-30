using Microsoft.AspNetCore.Mvc;
using Graphitie.Services;

namespace Graphitie.Controllers.Microsoft;

[ApiController]
[Route("[controller]")]
public class OwnersController : ControllerBase
{
    private readonly ILogger<OwnersController> _logger;

    private readonly MicrosoftService _microsoftService;

    public OwnersController(ILogger<OwnersController> logger, MicrosoftService microsoftService)
    {
        _logger = logger;
        _microsoftService = microsoftService;
    }
    
    [HttpPost(Name = "AddOwner")]
    public async Task<IActionResult> AddOwner(string siteId, string userId)
    {
        await _microsoftService.AddSiteOwner(siteId, userId);

        return new OkResult();

    }

    [HttpDelete(Name = "DeleteOwner")]
    public async Task<IActionResult> DeleteOwner(string siteId, string userId)
    {
        await _microsoftService.DeleteSiteOwner(siteId, userId);

        return new OkResult();
    }
}
