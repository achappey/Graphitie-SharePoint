using Microsoft.SharePoint.Client;

namespace Graphitie.Extensions;

public static class SharePointExtensions
{
    public static async Task SetDefaultContentType(this ClientContext context, string listTitle, string contentType)
    {

        var list = context.Web.GetListByTitle(listTitle);
        context.Load(list);

        var contentTypes = list.ContentTypes;
        context.Load(contentTypes);

        await context.ExecuteQueryRetryAsync();

        if (contentTypes.Select(e => e.Name).Contains(contentType))
        {
            list.SetDefaultContentType(contentTypes.First(e => e.Name == contentType).StringId);
        }
    }

    public static async Task AddContentTypeToList(this ClientContext context, string listTitle, string contentType)
    {
        var list = context.Web.GetListByTitle(listTitle);

        context.Load(list);

        await context.ExecuteQueryRetryAsync();

        if (!list.ContentTypesEnabled)
        {
            list.ContentTypesEnabled = true;

            list.Update();

            await context.ExecuteQueryRetryAsync();

        }

        list.AddContentTypeToListByName(contentType);
    }

    public static void RemoveContentTypeFromList(this ClientContext context, string listTitle, string contentType, bool ignoreInUseException = false)
    {
        var list = context.Web.GetListByTitle(listTitle);

        context.Load(list);

        try
        {
            list.RemoveContentTypeByName(contentType);
        }
        catch (Exception e)
        {
            if (!ignoreInUseException)
            {
                throw e;
            }
        }
    }

    public static async Task SupplyContentType(this ClientContext context, string item)
    {
        var contentTypes = context.Web.ContentTypes;

        context.Load(contentTypes, u => u.Include(a => a.StringId));

        await context.ExecuteQueryRetryAsync();

        if (!contentTypes.Select(t => t.StringId).Contains(item))
        {
            var sub = new Microsoft.SharePoint.Client.Taxonomy.ContentTypeSync.ContentTypeSubscriber(context);
            context.Load(sub);

            await context.ExecuteQueryRetryAsync();

            var res = sub.SyncContentTypesFromHubSite2(
                context.Url,
                new List<string>() {
                    item
                });

            await context.ExecuteQueryRetryAsync();
        }
    }

    public static async Task<string?> GetContentTypeId(this ClientContext context, string name)
    {
        var web = context.Web;
        var contenttypes = web.ContentTypes;

        context.Load(contenttypes);

        await context.ExecuteQueryAsync();

        return contenttypes.Where(d => name.ToLowerInvariant() == d.Name.ToLowerInvariant())
        .Select(d => d.StringId)
        .FirstOrDefault();
    }

    public static async Task<IEnumerable<string>> GetContentTypeNames(this ClientContext context)
    {
        var contentTypes = context.Web.ContentTypes;

        context.Load(contentTypes, u => u.Include(a => a.Name));

        await context.ExecuteQueryRetryAsync();

        return contentTypes.Select(a => a.Name);
    }

    public static async Task<Tuple<string, byte[]>> GetFile(this ClientContext context, string fileUrl)
    {
        var file = context.Web.GetFileByServerRelativeUrl(fileUrl);
        context.Load(file);

        var stream = file.OpenBinaryStream();

        await context.ExecuteQueryRetryAsync();

        using (MemoryStream memoryStream = new MemoryStream())
        {
            await stream.Value.CopyToAsync(memoryStream);

            return Tuple.Create(file.Name, memoryStream.ToArray());
        }
    }

    public static async Task<Microsoft.SharePoint.Client.File> UploadToSiteAssets(this ClientContext context, string fileName, byte[] file)
    {
        var list = context.Web.Lists.EnsureSiteAssetsLibrary();
        context.Load(list, i => i.RootFolder);

        await context.ExecuteQueryRetryAsync();

        var folder = context.Web.GetFolderById(list.RootFolder.UniqueId);

        using (MemoryStream stream = new MemoryStream(file))
        {
            return await folder.UploadFileAsync(fileName, stream, true);
        }
    }
}