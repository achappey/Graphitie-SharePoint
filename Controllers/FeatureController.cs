using Microsoft.AspNetCore.Mvc;
using Graphitie.Services;
using Microsoft.AspNetCore.Authorization;

namespace Graphitie.Controllers.Microsoft;

[ApiController]
[Route("[controller]")]
[Authorize]
public class FeatureController : ControllerBase
{
    private readonly ILogger<FeatureController> _logger;

    private readonly GraphitieService _graphitieService;

    public FeatureController(ILogger<FeatureController> logger, GraphitieService graphitieService)
    {
        _logger = logger;
        _graphitieService = graphitieService;
    }
    
    [HttpPost(Name = "ActivateFeature")]
    public async Task ActivateFeature(string siteUrl, string featureId)
    {
        await _graphitieService.ActivateFeature(siteUrl, featureId);

    }
}
