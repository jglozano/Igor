namespace Igor
{
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.Management.WebSites;

    public interface IClientHelper
    {
        SubscriptionCloudCredentials Credentials { get; }
        WebSiteManagementClient GetWebSiteClient();
    }
}