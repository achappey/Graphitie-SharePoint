
namespace Graphitie.Extensions;

public static class CoreExtensions
{
    public static string ExtractSiteUrl(this string url)
    {
        return $"/sites/{url.Split('/')[2]}";
    }


}
