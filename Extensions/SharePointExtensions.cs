using Microsoft.SharePoint.Client;

namespace Graphitie.Extensions;

public static class SharePointExtensions
{
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