using AutoMapper;

namespace Graphitie.Services;

public interface IGraphitieService
{
    public Task AddVisitor(string siteUrl, string userId);
    public Task AddMember(string siteUrl, string userId);
    public Task DeleteMember(string siteUrl, string userId);
    public Task AddLogo(string logoUrl, string siteUrl);
    public Task AddTheme(string themeUrl, string siteUrl);
    public Task AddQuickLaunchLinkAsync(string siteUrl, string name, string link, string previousLinkName = "");
    public Task ActivateFeature(string siteUrl, string featureId);

}

public class GraphitieService : IGraphitieService
{
    private readonly Microsoft.Graph.GraphServiceClient _graphServiceClient;
    private readonly IMicrosoftService _microsoftService;
    private readonly IMapper _mapper;

    public GraphitieService(Microsoft.Graph.GraphServiceClient graphServiceClient,
    IMapper mapper,
    MicrosoftService microsoftService)
    {
        _graphServiceClient = graphServiceClient;
        _microsoftService = microsoftService;
        _mapper = mapper;
    }


    public async Task AddVisitor(string siteId, string userId)
    {
        await _microsoftService.AddSiteVisitor(siteId, userId);
    }

    public async Task AddLogo(string logoUrl, string siteUrl)
    {
        await _microsoftService.AddSiteLogo(logoUrl, siteUrl);
    }

    public async Task AddTheme(string logoUrl, string siteUrl)
    {
        await _microsoftService.AddSiteTheme(logoUrl, siteUrl);
    }

    public async Task AddMember(string siteId, string userId)
    {
        await _microsoftService.AddSiteMember(siteId, userId);
    }
    public async Task DeleteMember(string siteId, string userId)
    {
        await _microsoftService.DeleteSiteMember(siteId, userId);
    }

    public async Task AddQuickLaunchLinkAsync(string siteUrl, string name, string link, string previousLinkName = "")
    {
        await _microsoftService.AddQuickLaunchLinkAsync(siteUrl, name, link, previousLinkName);
    }

    public async Task ActivateFeature(string siteUrl, string featureId)

    {
        await _microsoftService.ActivateFeature(siteUrl, featureId);
    }

}