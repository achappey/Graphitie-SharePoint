
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

public class GraphitieService(MicrosoftService microsoftService) : IGraphitieService
{
    private readonly IMicrosoftService _microsoftService = microsoftService;

    public Task AddVisitor(string siteId, string userId)
    {
        return _microsoftService.AddSiteVisitor(siteId, userId);
    }

    public Task AddLogo(string logoUrl, string siteUrl)
    {
        return _microsoftService.AddSiteLogo(logoUrl, siteUrl);
    }

    public Task AddTheme(string logoUrl, string siteUrl)
    {
        return _microsoftService.AddSiteTheme(logoUrl, siteUrl);
    }

    public Task AddMember(string siteId, string userId)
    {
        return _microsoftService.AddSiteMember(siteId, userId);
    }
    public Task DeleteMember(string siteId, string userId)
    {
        return _microsoftService.DeleteSiteMember(siteId, userId);
    }

    public Task AddQuickLaunchLinkAsync(string siteUrl, string name, string link, string previousLinkName = "")
    {
        return _microsoftService.AddQuickLaunchLinkAsync(siteUrl, name, link, previousLinkName);
    }

    public Task ActivateFeature(string siteUrl, string featureId)

    {
        return _microsoftService.ActivateFeature(siteUrl, featureId);
    }

}