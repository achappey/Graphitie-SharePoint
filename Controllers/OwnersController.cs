using Microsoft.AspNetCore.Mvc;
using Graphitie.Services;

namespace Graphitie.Controllers.Microsoft;

[ApiController]
[Route("[controller]")]
public class OwnersController(ILogger<OwnersController> logger, MicrosoftService microsoftService) : ControllerBase
{
    private readonly ILogger<OwnersController> _logger = logger;

    private readonly MicrosoftService _microsoftService = microsoftService;

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
