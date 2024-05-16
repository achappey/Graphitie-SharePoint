using Microsoft.AspNetCore.Mvc;
using Graphitie.Services;

namespace Graphitie.Controllers.Microsoft;

[ApiController]
[Route("[controller]")]
public class FeatureController(ILogger<FeatureController> logger, GraphitieService graphitieService) : ControllerBase
{
    private readonly ILogger<FeatureController> _logger = logger;

    private readonly GraphitieService _graphitieService = graphitieService;

    [HttpPost(Name = "ActivateFeature")]
    public async Task ActivateFeature(string siteUrl, string featureId)
    {
        await _graphitieService.ActivateFeature(siteUrl, featureId);

    }
}
