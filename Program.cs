using Microsoft.OpenApi.Models;
using Azure.Identity;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using Graphitie.Services;
using Graphitie;

var odataEndpoint = "odata";
var version = "v1";

var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);
var appConfig = builder.Configuration.Get<AppConfig>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(version, new OpenApiInfo { Title = appConfig.SharePoint.TenantName, Version = version });

    c.DocInclusionPredicate((docName, apiDesc) =>
    {
        return apiDesc.RelativePath != null ? docName == odataEndpoint ?
            apiDesc.RelativePath.Contains(odataEndpoint) : !apiDesc.RelativePath.Contains(odataEndpoint) : false;
    });
});

builder.Services.AddAutoMapper(
          typeof(Graphitie.Profiles.Microsoft.MicrosoftProfile)
      );

builder.Services.AddScoped<GraphitieService>();
builder.Services.AddScoped<MicrosoftService>();
builder.Services.AddScoped<SharePointService>(y => new SharePointService(appConfig.SharePoint.TenantName, appConfig.SharePoint.ClientId, appConfig.SharePoint.ClientSecret));

builder.Services.AddHttpClient();

builder.Services.AddControllers();

var microsoft = builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
          .AddMicrosoftIdentityWebApp(builder.Configuration)
          .EnableTokenAcquisitionToCallDownstreamApi()
            .AddMicrosoftGraphAppOnly(authenticationProvider => new GraphServiceClient(new ClientSecretCredential(appConfig.AzureAd.TenantId,
                appConfig.AzureAd.ClientId,
                appConfig.AzureAd.ClientSecret)))      
          .AddInMemoryTokenCaches();

var app = builder.Build();

app.UseSwagger(options =>
{
    options.SerializeAsV2 = true;
});

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint(
        string.Format("/swagger/{0}/swagger.json", version),
        string.Format("{0} {1}", appConfig.SharePoint.TenantName, version));
});

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
