using Microsoft.AspNetCore.Mvc;
using Graphitie.Services;

namespace Graphitie.Controllers.Microsoft;

[ApiController]
[Route("[controller]")]
public class ThemeController(ILogger<ThemeController> logger, GraphitieService graphitieService) : ControllerBase
{
    private readonly ILogger<ThemeController> _logger = logger;

    private readonly GraphitieService _graphitieService = graphitieService;

    [HttpPost(Name = "AddTheme")]
    public async Task AddTheme(string themeUrl, string targetSite)
    {
        await _graphitieService.AddTheme(themeUrl, targetSite);

    }
}
