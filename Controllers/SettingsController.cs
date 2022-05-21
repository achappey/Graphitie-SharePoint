using Microsoft.AspNetCore.Mvc;
using Graphitie.Services;
using Microsoft.AspNetCore.Authorization;

namespace Graphitie.Controllers.Microsoft;

[ApiController]
[Route("[controller]")]
[Authorize]
public class SettingsController : ControllerBase
{
    private readonly ILogger<SettingsController> _logger;

    private readonly MicrosoftService _microsoftService;

    public SettingsController(ILogger<SettingsController> logger, MicrosoftService microsoftService)
    {
        _logger = logger;
        _microsoftService = microsoftService;
    }
    
    [HttpPost(Name = "Rename")]
    public async Task Rename(string siteUrl, string name)
    {
        await _microsoftService.RenameSite(siteUrl, name);

    }
}
