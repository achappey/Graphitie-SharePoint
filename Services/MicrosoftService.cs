using Graphitie.Extensions;
using Microsoft.SharePoint.Client;

namespace Graphitie.Services;

public interface IMicrosoftService
{
    public Task AddSiteLogo(string logo, string targetSite);
    public Task AddSiteTheme(string theme, string targetSite);
    public Task AddSiteOwner(string siteId, string userId);
    public Task AddSiteVisitor(string siteId, string userId);
    public Task AddSiteMember(string siteId, string userId);
    public Task DeleteSiteMember(string siteId, string userId);
    public Task DeleteSiteOwner(string siteId, string userId);
    public Task AddQuickLaunchLink(string siteUrl, string name, string link, string previousLinkName = "");
    public Task ActivateFeature(string siteUrl, string featureId);
    public Task RenameSite(string siteUrl, string name);
    public Task AddContentType(string siteUrl, string contentType, string listName);
    public void DeleteContentType(string siteUrl, string contentType, string listName, bool ignoreInUseException = false);
}

public class MicrosoftService : IMicrosoftService
{
    private readonly Microsoft.Graph.GraphServiceClient _graphServiceClient;
    private readonly SharePointService _sharePointService;

    public MicrosoftService(Microsoft.Graph.GraphServiceClient graphServiceClient, SharePointService sharePointService)
    {
        _graphServiceClient = graphServiceClient;
        _sharePointService = sharePointService;
    }

    public void DeleteContentType(string siteUrl, string contentType, string listName, bool ignoreInUseException = false)
    {
        using var context = this._sharePointService.GetContext(siteUrl);

        context.RemoveContentTypeFromList(listName, contentType, ignoreInUseException);
    }

    public async Task SetDefaultContentType(string siteUrl, string contentType, string listName)
    {
        using var context = this._sharePointService.GetContext(siteUrl);

        await context.SetDefaultContentType(listName, contentType);
        
    }

    public async Task AddContentType(string siteUrl, string contentType, string listName)
    {
        using var context = this._sharePointService.GetContext(siteUrl);
        var currentContentTypes = await context.GetContentTypeNames();

        if (!currentContentTypes.Contains(contentType))
        {
            using (var hubContext = this._sharePointService.GetContext("contentTypeHub"))
            {
                var contentTypeId = await hubContext.GetContentTypeId(contentType);

                if (contentTypeId != null)
                {
                    await context.SupplyContentType(contentTypeId);
                    
                }
            }
        }

        await context.AddContentTypeToList(listName, contentType);
    }



    public async Task ActivateFeature(string siteUrl, string featureId)
    {
        var guid = new Guid(featureId);
        using var context = this._sharePointService.GetContext(siteUrl);
        var active = await context.Site.IsFeatureActiveAsync(guid);

        if (!active)
        {
            await context.Site.ActivateFeatureAsync(guid);
        }

    }

    public async Task AddQuickLaunchLink(string siteUrl, string name, string link, string previousLinkName = "")
    {
        using var context = this._sharePointService.GetContext(siteUrl);
        var quickLaunch = context.Web.Navigation.QuickLaunch;

        context.Load(quickLaunch);

        await context.ExecuteQueryRetryAsync();

        if (quickLaunch.All(r => r.Url != link))
        {
            NavigationNodeCreationInformation newNode = new NavigationNodeCreationInformation()
            {
                Title = name,
                Url = link
            };

            if (!string.IsNullOrEmpty(previousLinkName))
            {
                newNode.PreviousNode = quickLaunch.FirstOrDefault(a => a.Title == previousLinkName);
            }

            quickLaunch.Add(newNode);

            await context.ExecuteQueryRetryAsync();
        }

    }

    public async Task AddSiteLogo(string logo, string targetSite)
    {
        using var context = this._sharePointService.GetContext(logo.ExtractSiteUrl());
        using var targetContext = this._sharePointService.GetContext(targetSite);
        var logoFile = await context.GetFile(logo);
        var file = await targetContext.UploadToSiteAssets(logoFile.Item1, logoFile.Item2);

        targetContext.Web.SiteLogoUrl = file.ServerRelativeUrl;
        targetContext.Web.Update();

        await targetContext.ExecuteQueryRetryAsync();

    }


    public async Task RenameSite(string siteUrl, string name)
    {
        using var targetContext = this._sharePointService.GetContext(siteUrl);

        targetContext.Web.Title = name;
        targetContext.Web.Update();

        await targetContext.ExecuteQueryRetryAsync();

    }
    public async Task AddSiteTheme(string themeUrl, string targetSite)
    {
        using var context = this._sharePointService.GetContext(themeUrl.ExtractSiteUrl());
        using var targetContext = this._sharePointService.GetContext(targetSite);
        var logoFile = await context.GetFile(themeUrl);
        var file = await targetContext.UploadToSiteAssets(logoFile.Item1, logoFile.Item2);

        targetContext.Web.ApplyTheme(file.ServerRelativeUrl, null, null, false);

        await targetContext.ExecuteQueryRetryAsync();
    }

    public async Task AddSiteVisitor(string siteId, string userId)
    {
        using var context = this._sharePointService.GetContext(siteId);
        var adGroup = context.Web.EnsureUser(userId);
        context.Load(adGroup);

        var spGroup = context.Web.AssociatedVisitorGroup;
        spGroup.Users.AddUser(adGroup);
        context.Load(spGroup, x => x.Users);

        await context.ExecuteQueryRetryAsync();
    }

    public async Task AddSiteOwner(string siteId, string userId)
    {
        using var context = this._sharePointService.GetContext(siteId);
        var adGroup = context.Web.EnsureUser(userId);
        context.Load(adGroup);

        var spGroup = context.Web.AssociatedOwnerGroup;
        spGroup.Users.AddUser(adGroup);
        context.Load(spGroup, x => x.Users);

        await context.ExecuteQueryRetryAsync();
    }

    public async Task AddSiteMember(string siteId, string userId)
    {
        using var context = this._sharePointService.GetContext(siteId);
        var adGroup = context.Web.EnsureUser(userId);
        context.Load(adGroup);

        var spGroup = context.Web.AssociatedMemberGroup;
        spGroup.Users.AddUser(adGroup);
        context.Load(spGroup, x => x.Users);

        await context.ExecuteQueryRetryAsync();
    }

    public async Task DeleteSiteMember(string siteId, string userId)
    {
        using (var context = this._sharePointService.GetContext(siteId))
        {
            var adGroup = context.Web.EnsureUser(userId);
            context.Load(adGroup);

            var spGroup = context.Web.AssociatedMemberGroup;
            spGroup.Users.Remove(adGroup);
            context.Load(spGroup, x => x.Users);

            await context.ExecuteQueryRetryAsync();
        }
    }


    public async Task DeleteSiteOwner(string siteId, string userId)
    {
        using (var context = this._sharePointService.GetContext(siteId))
        {
            var adGroup = context.Web.EnsureUser(userId);
            context.Load(adGroup);

            var spGroup = context.Web.AssociatedOwnerGroup;
            spGroup.Users.Remove(adGroup);
            context.Load(spGroup, x => x.Users);

            await context.ExecuteQueryRetryAsync();
        }
    }
}