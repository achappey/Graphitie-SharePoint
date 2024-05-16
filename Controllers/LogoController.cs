using Microsoft.AspNetCore.Mvc;
using Graphitie.Services;

namespace Graphitie.Controllers.Microsoft;

[ApiController]
[Route("[controller]")]
public class LogoController(ILogger<LogoController> logger, GraphitieService graphitieService) : ControllerBase
{
    private readonly ILogger<LogoController> _logger = logger;

    private readonly GraphitieService _graphitieService = graphitieService;

    [HttpPost(Name = "AddLogo")]
    public async Task AddLogo(string logoUrl, string targetSite)
    {
        await _graphitieService.AddLogo(logoUrl, targetSite);

    }
}
