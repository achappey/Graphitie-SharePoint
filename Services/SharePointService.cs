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

    private string ToTenantUrl(string site) {
            return string.IsNullOrEmpty(site) ? BaseUrl : site.StartsWith("/sites/") ? string.Format("{0}{1}", this.BaseUrl, site) : string.Format("{0}/sites/{1}", this.BaseUrl, site);
    }

    public string GetBaseUrl() {
            return this.BaseUrl;
    }

    private string BaseUrl {
        get {
            return string.Format("https://{0}.sharepoint.com",this._tenantName);
        }
    }

    public SharePointService(string tenantName, string clientId, string clientSecret) {
        this._tenantName = tenantName;
        this._clientId = clientId;
        this._clientSecret = clientSecret;
    }

    public ClientContext GetContext(string url) {
        AuthenticationManager authManager = new AuthenticationManager();

        return authManager.GetACSAppOnlyContext(this.ToTenantUrl(url), this._clientId, this._clientSecret);
    }
   
}
