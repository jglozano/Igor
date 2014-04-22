namespace Igor.Helpers
{
    using Microsoft.WindowsAzure.Management.WebSites;
    using Microsoft.WindowsAzure.WebSitesExtensions;

    public interface IClientHelper
    {
        WebSiteManagementClient GetWebSiteClient();
        WebSiteExtensionsClient GetWebSiteExtensionsClient(string siteName, string userName, string password);
    }
}