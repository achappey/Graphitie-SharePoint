using Microsoft.AspNetCore.Mvc;
using Graphitie.Services;
using Microsoft.AspNetCore.Authorization;

namespace Graphitie.Controllers.Microsoft;

[ApiController]
[Route("[controller]")]
[Authorize]
public class LogoController : ControllerBase
{
    private readonly ILogger<LogoController> _logger;

    private readonly GraphitieService _graphitieService;

    public LogoController(ILogger<LogoController> logger, GraphitieService graphitieService)
    {
        _logger = logger;
        _graphitieService = graphitieService;
    }
    
    [HttpPost(Name = "AddLogo")]
    public async Task AddLogo(string logoUrl, string targetSite)
    {
        await _graphitieService.AddLogo(logoUrl, targetSite);

    }
}
