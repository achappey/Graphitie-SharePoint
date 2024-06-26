using Microsoft.AspNetCore.Mvc;
using Graphitie.Services;

namespace Graphitie.Controllers.Microsoft;

[ApiController]
[Route("[controller]")]
public class MembersController(ILogger<MembersController> logger, GraphitieService graphitieService) : ControllerBase
{
    private readonly ILogger<MembersController> _logger = logger;

    private readonly GraphitieService _graphitieService = graphitieService;

    [HttpPost(Name = "AddMember")]
    public async Task AddMember(string siteId, string userId)
    {
        await _graphitieService.AddMember(siteId, userId);

    }

    [HttpDelete(Name = "DeleteMember")]
    public async Task DeleteMember(string siteId, string userId)
    {
        await _graphitieService.DeleteMember(siteId, userId);

    }
}
