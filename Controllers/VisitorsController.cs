using Microsoft.AspNetCore.Mvc;
using Graphitie.Services;

namespace Graphitie.Controllers.Microsoft;

[ApiController]
[Route("[controller]")]
public class VisitorsController(ILogger<VisitorsController> logger, GraphitieService graphitieService) : ControllerBase
{
    private readonly ILogger<VisitorsController> _logger = logger;

    private readonly GraphitieService _graphitieService = graphitieService;

    [HttpPost(Name = "AddVisitor")]
    public async Task AddVisitor(string siteId, string userId)
    {
        await _graphitieService.AddVisitor(siteId, userId);

    }

}
