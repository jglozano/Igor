namespace Igor.Helpers
{
    using System;
    using System.Configuration;
    using System.Security.Cryptography.X509Certificates;
    using Annotations;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.Management.WebSites;
    using Microsoft.WindowsAzure.WebSitesExtensions;

    [UsedImplicitly]
    public class ClientHelper : IClientHelper
    {
        public WebSiteManagementClient GetWebSiteClient()
        {
            var subscriptionId = CloudConfigurationManager.GetSetting("SubscriptionId");
            var certificateBase64 = CloudConfigurationManager.GetSetting("ManagementCertificate");
            if (string.IsNullOrWhiteSpace(certificateBase64))
            {
                throw new ConfigurationErrorsException("ManagementCertificate not set");
            }
            var certificateBytes = Convert.FromBase64String(certificateBase64);
            var certificate = new X509Certificate2(certificateBytes);
            return CloudContext.Clients.CreateWebSiteManagementClient(new CertificateCloudCredentials(subscriptionId, certificate));
        }

        public WebSiteExtensionsClient GetWebSiteExtensionsClient(string siteName, string userName, string password)
        {
            return
                CloudContext.Clients.CreateWebSiteExtensionsClient(
                    new BasicAuthenticationCloudCredentials {Username = userName, Password = password}, siteName);
        }
    }
}