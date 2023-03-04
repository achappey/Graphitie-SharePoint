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

    private string ToTenantUrl(string site)
    {
        return string.IsNullOrWhiteSpace(site) ? BaseUrl : $"{BaseUrl}/sites/{site}";
    }


    public string GetBaseUrl()
    {
        return this.BaseUrl;
    }

    private string BaseUrl
    {
        get
        {
            return $"https://{this._tenantName}.sharepoint.com";
        }
    }

    public SharePointService(string tenantName, string clientId, string clientSecret)
    {
        this._tenantName = tenantName;
        this._clientId = clientId;
        this._clientSecret = clientSecret;
    }

    public ClientContext GetContext(string url)
    {
        AuthenticationManager authManager = new AuthenticationManager();

        return authManager.GetACSAppOnlyContext(this.ToTenantUrl(url), this._clientId, this._clientSecret);
    }

}
