using Microsoft.AspNetCore.Mvc;
using Graphitie.Services;
using Microsoft.AspNetCore.Authorization;

namespace Graphitie.Controllers.Microsoft;

[ApiController]
[Route("[controller]")]
[Authorize]
public class QuickLaunchController : ControllerBase
{
    private readonly ILogger<QuickLaunchController> _logger;

    private readonly GraphitieService _graphitieService;

    public QuickLaunchController(ILogger<QuickLaunchController> logger, GraphitieService graphitieService)
    {
        _logger = logger;
        _graphitieService = graphitieService;
    }
    
    [HttpPost(Name = "AddLink")]
    public async Task AddLink(string siteUrl, string name, string link, string previousLinkName = "")
    {
        await _graphitieService.AddQuickLaunchLink(siteUrl, name, link, previousLinkName);

    }
}
