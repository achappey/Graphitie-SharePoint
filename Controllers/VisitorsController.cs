using Microsoft.AspNetCore.Mvc;
using Graphitie.Services;

namespace Graphitie.Controllers.Microsoft;

[ApiController]
[Route("[controller]")]
public class VisitorsController : ControllerBase
{
    private readonly ILogger<VisitorsController> _logger;

    private readonly GraphitieService _graphitieService;

    public VisitorsController(ILogger<VisitorsController> logger, GraphitieService graphitieService)
    {
        _logger = logger;
        _graphitieService = graphitieService;
    }

    [HttpPost(Name = "AddVisitor")]
    public async Task AddVisitor(string siteId, string userId)
    {
        await _graphitieService.AddVisitor(siteId, userId);

    }

}
