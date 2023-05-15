using Microsoft.AspNetCore.Mvc;
using Graphitie.Services;
using System.ComponentModel.DataAnnotations;

namespace Graphitie.Controllers.Microsoft;

[ApiController]
[Route("[controller]")]
public class QuickLaunchController : ControllerBase
{
    private readonly ILogger<QuickLaunchController> _logger;

    private readonly GraphitieService _graphitieService;

    public QuickLaunchController(ILogger<QuickLaunchController> logger, GraphitieService graphitieService)
    {
        _logger = logger;
        _graphitieService = graphitieService;
    }

    /// <summary>
    /// Adds a new quick launch link to the specified SharePoint site.
    /// </summary>
    /// <param name="siteUrl">The URL of the SharePoint site.</param>
    /// <param name="name">The display name of the new quick launch link.</param>
    /// <param name="link">The URL of the new quick launch link.</param>
    /// <param name="previousLinkName">The display name of the link that the new link should be placed after (optional).</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    [HttpPost(Name = "AddLink")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddLink(
        [Required] string siteUrl,
        [Required] string name,
        [Required] string link,
        string previousLinkName = "")
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _graphitieService.AddQuickLaunchLinkAsync(siteUrl, name, link, previousLinkName);
        return NoContent();
    }
}
