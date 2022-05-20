

namespace Graphitie;

public class AppConfig
{
    public SharePoint SharePoint { get; set; } = null!;
    public AzureAd AzureAd { get; set; } = null!;
}

public class AzureAd
{
    public string Instance { get; set; } = null!;
    public string ClientId { get; set; } = null!;
    public string ClientSecret { get; set; } = null!;
    public string TenantId { get; set; } = null!;

}

public class SharePoint
{
    public string ClientId { get; set; } = null!;
    public string ClientSecret { get; set; } = null!;
    public string TenantName { get; set; } = null!;

}