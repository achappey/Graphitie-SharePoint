using Microsoft.AspNetCore.Mvc;
using Graphitie.Services;
using Microsoft.AspNetCore.Authorization;

namespace Graphitie.Controllers.Microsoft;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ThemeController : ControllerBase
{
    private readonly ILogger<ThemeController> _logger;

    private readonly GraphitieService _graphitieService;

    public ThemeController(ILogger<ThemeController> logger, GraphitieService graphitieService)
    {
        _logger = logger;
        _graphitieService = graphitieService;
    }
    
    [HttpPost(Name = "AddTheme")]
    public async Task AddTheme(string themeUrl, string targetSite)
    {
        await _graphitieService.AddTheme(themeUrl, targetSite);

    }
}
