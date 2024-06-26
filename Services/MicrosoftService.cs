using Graphitie.Extensions;
using Graphitie.Models;
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
    public Task AddQuickLaunchLinkAsync(string siteUrl, string name, string link, string previousLinkName = "");
    public Task ActivateFeature(string siteUrl, string featureId);
    public Task RenameSite(string siteUrl, string name);
    public Task AddContentType(string siteUrl, string contentType, string listName);
    public void DeleteContentType(string siteUrl, string contentType, string listName, bool ignoreInUseException = false);
    public Task<List<ContentTypeDetails>> GetAllContentTypes(string siteUrl);
}

public class MicrosoftService(SharePointService sharePointService) : IMicrosoftService
{
    private readonly SharePointService _sharePointService = sharePointService;

    public void DeleteContentType(string siteUrl, string contentType, string listName, bool ignoreInUseException = false)
    {
        using var context = _sharePointService.GetContext(siteUrl);

        context.RemoveContentTypeFromList(listName, contentType, ignoreInUseException);
    }

    public Task SetDefaultContentType(string siteUrl, string contentType, string listName)
    {
        using var context = _sharePointService.GetContext(siteUrl);

        return context.SetDefaultContentType(listName, contentType);

    }
    public Task<List<string>> GetAllDocumentLibraries(string siteUrl)
    {
        using var context = _sharePointService.GetContext(siteUrl);

        return context.GetAllDocumentLibraryTitles();

    }


    public Task<List<ContentTypeDetails>> GetAllContentTypes(string siteUrl)
    {
        using var context = _sharePointService.GetContext(siteUrl);

        return context.GetAllContentTypes();

    }

    public async Task AddContentType(string siteUrl, string contentType, string listName)
    {
        using var context = _sharePointService.GetContext(siteUrl);

        var currentContentTypes = await context.GetContentTypeNames();

        if (!currentContentTypes.Contains(contentType))
        {
            var hubContext = _sharePointService.GetContext("contentTypeHub");

            var contentTypeId = await hubContext.GetContentTypeId(contentType);

            if (contentTypeId != null)
            {
                await context.SupplyContentType(contentTypeId);
            }
        }

        await context.AddContentTypeToList(listName, contentType);
    }

    public async Task ActivateFeature(string siteUrl, string featureId)
    {
        var guid = new Guid(featureId);
        using var context = _sharePointService.GetContext(siteUrl);
        var active = await context.Site.IsFeatureActiveAsync(guid);

        if (!active)
        {
            await context.Site.ActivateFeatureAsync(guid);
        }

    }

    /// <summary>
    /// Adds a new quick launch link to the specified SharePoint site.
    /// </summary>
    /// <param name="siteUrl">The URL of the SharePoint site.</param>
    /// <param name="name">The display name of the new quick launch link.</param>
    /// <param name="link">The URL of the new quick launch link.</param>
    /// <param name="previousLinkName">The display name of the link that the new link should be placed after (optional).</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task AddQuickLaunchLinkAsync(string siteUrl, string name, string link, string previousLinkName = "")
    {
        if (string.IsNullOrEmpty(siteUrl)) throw new ArgumentNullException(nameof(siteUrl));
        if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
        if (string.IsNullOrEmpty(link)) throw new ArgumentNullException(nameof(link));

        using var context = _sharePointService.GetContext(siteUrl);
        var quickLaunch = context.Web.Navigation.QuickLaunch;
        context.Load(quickLaunch);
        await context.ExecuteQueryRetryAsync();

        if (quickLaunch.Any(r => r.Url == link)) return;

        NavigationNodeCreationInformation newNode = new()
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


    public async Task AddSiteLogo(string logo, string targetSite)
    {
        using var context = _sharePointService.GetContext(logo.ExtractSiteUrl());
        using var targetContext = _sharePointService.GetContext(targetSite);
        var logoFile = await context.GetFile(logo);
        var file = await targetContext.UploadToSiteAssets(logoFile.Item1, logoFile.Item2);
        targetContext.Web.SiteLogoUrl = file.ServerRelativeUrl;
        targetContext.Web.Update();
        await targetContext.ExecuteQueryRetryAsync();
    }


    public Task RenameSite(string siteUrl, string name)
    {
        using var targetContext = _sharePointService.GetContext(siteUrl);
        targetContext.Web.Title = name;
        targetContext.Web.Update();

        return targetContext.ExecuteQueryRetryAsync();

    }
    public async Task AddSiteTheme(string themeUrl, string targetSite)
    {
        using var context = _sharePointService.GetContext(themeUrl.ExtractSiteUrl());
        using var targetContext = _sharePointService.GetContext(targetSite);
        var logoFile = await context.GetFile(themeUrl);
        var file = await targetContext.UploadToSiteAssets(logoFile.Item1, logoFile.Item2);

        targetContext.Web.ApplyTheme(file.ServerRelativeUrl, null, null, false);

        await targetContext.ExecuteQueryRetryAsync();
    }


    public Task AddSiteVisitor(string siteId, string userId)
    {
        using var context = _sharePointService.GetContext(siteId);
        var user = context.Web.EnsureUser(userId);
        var spGroup = context.Web.AssociatedVisitorGroup;
        spGroup.Users.AddUser(user);
        return context.ExecuteQueryRetryAsync();
    }

    public Task AddSiteOwner(string siteId, string userId)
    {
        using var context = _sharePointService.GetContext(siteId);
        var user = context.Web.EnsureUser(userId);
        var spGroup = context.Web.AssociatedOwnerGroup;
        spGroup.Users.AddUser(user);
        return context.ExecuteQueryRetryAsync();
    }

    public Task AddSiteMember(string siteId, string userId)
    {

        using var context = _sharePointService.GetContext(siteId);
        var user = context.Web.EnsureUser(userId);
        var spGroup = context.Web.AssociatedMemberGroup;
        spGroup.Users.AddUser(user);
        return context.ExecuteQueryRetryAsync();

    }

    public Task DeleteSiteMember(string siteId, string userId)
    {
        using var context = _sharePointService.GetContext(siteId);
        var user = context.Web.EnsureUser(userId);
        var spGroup = context.Web.AssociatedMemberGroup;
        spGroup.Users.Remove(user);
        return context.ExecuteQueryRetryAsync();
    }


    public Task DeleteSiteOwner(string siteId, string userId)
    {
        using var context = _sharePointService.GetContext(siteId);
        var user = context.Web.EnsureUser(userId);
        var spGroup = context.Web.AssociatedOwnerGroup;
        spGroup.Users.Remove(user);
        return context.ExecuteQueryRetryAsync();
    }
}