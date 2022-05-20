using Microsoft.AspNetCore.Mvc;
using Graphitie.Services;
using Microsoft.AspNetCore.Authorization;

namespace Graphitie.Controllers.Microsoft;

[ApiController]
[Route("[controller]")]
[Authorize]
public class GroupController : ControllerBase
{
    private readonly ILogger<GroupController> _logger;

    private readonly GraphitieService _graphitieService;

    public GroupController(ILogger<GroupController> logger, GraphitieService graphitieService)
    {
        _logger = logger;
        _graphitieService = graphitieService;
    }
    
    [HttpGet(Name = "AddGroupLogo")]
    public async Task AddGroupLogo(string groupId, string logoUrl)
    {
        await _graphitieService.UpdateGroupPhoto(groupId, logoUrl);

    }
}
