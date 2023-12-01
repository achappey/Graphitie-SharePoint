using Microsoft.SharePoint.Client;

using PnP.Framework;

namespace Graphitie.Services;

public interface ISharePointService
{
    public ClientContext GetContext(string url);
    public string GetBaseUrl();

}

public class SharePointService : ISharePointService
{
    private readonly string _tenantName;
    private readonly string _clientId;
    private readonly string _clientSecret;

    /// <summary>
    /// Converts a site path to a tenant URL by appending it to the base URL.
    /// </summary>
    /// <param name="site">The site path to convert.</param>
    /// <returns>The tenant URL.</returns>
    private string ToTenantUrl(string site)
    {
        if (string.IsNullOrWhiteSpace(site))
        {
            return BaseUrl;
        }

        if (site.StartsWith("/sites/", StringComparison.OrdinalIgnoreCase))
        {
            return $"{BaseUrl}{site}";
        }

        return $"{BaseUrl}/sites/{site}";
    }


    public string GetBaseUrl()
    {
        return BaseUrl;
    }

    private string BaseUrl
    {
        get
        {
            return $"https://{_tenantName}.sharepoint.com";
        }
    }

    public SharePointService(string tenantName, string clientId, string clientSecret)
    {
        _tenantName = tenantName;
        _clientId = clientId;
        _clientSecret = clientSecret;
    }

    public ClientContext GetContext(string url)
    {
        AuthenticationManager authManager = new();

        return authManager.GetACSAppOnlyContext(ToTenantUrl(url), _clientId, _clientSecret);
    }

}
