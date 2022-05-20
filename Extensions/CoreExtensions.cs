
namespace Graphitie.Extensions;

public static class CoreExtensions
{
    public static string ExtractSiteUrl(this string url)
    {
        var items = url.Split('/');

        return string.Format("/sites/{0}", items[2]);
    }

}
