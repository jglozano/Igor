namespace Igor
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Net.PeerToPeer;
    using System.Security.Cryptography.X509Certificates;
    using System.Security.Policy;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.Management.WebSites;
    using Microsoft.WindowsAzure.WebSitesExtensions;

    public class ClientHelper : IClientHelper
    {
        private static readonly object Sync = new object();
        private static X509Certificate2 _certificate;
        private static string _subscriptionId;
        private static string _userName;
        private static string _password;

        public SubscriptionCloudCredentials Credentials
        {
            get
            {
                if (_certificate == null)
                {
                    LoadSettings();
                }
                return new CertificateCloudCredentials(_subscriptionId, _certificate);
            }
        }

        public WebSiteManagementClient GetWebSiteClient()
        {
            return CloudContext.Clients.CreateWebSiteManagementClient(Credentials);
        }

        public WebSiteExtensionsClient GetWebSiteExtensionsClient(string siteName)
        {
            return
                CloudContext.Clients.CreateWebSiteExtensionsClient(
                    new BasicAuthenticationCloudCredentials {Username = _userName, Password = _password}, siteName);
        }

        private static void LoadSettings()
        {
            lock (Sync)
            {
                if (_certificate != null) return;
                var connectionString = GetConnectionStringFromConfig();
                var dict = ParseConnectionString(connectionString);
                ValidateSettings(dict);
                _subscriptionId = dict["SubscriptionId"];
                var certificateBytes = Convert.FromBase64String(dict["ManagementCertificate"]);
                _certificate = new X509Certificate2(certificateBytes, "", X509KeyStorageFlags.UserKeySet | X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);
                _userName = dict["UserName"];
                _password = dict["Password"];
            }
        }

        private static void ValidateSettings(Dictionary<string, string> dict)
        {
            if (!dict.ContainsKey("UserName"))
                throw new ConfigurationErrorsException("Credentials connectionString missing UserName");
            if (!dict.ContainsKey("Password"))
                throw new ConfigurationErrorsException("Credentials connectionString missing Password");
            if (!dict.ContainsKey("SubscriptionId"))
                throw new ConfigurationErrorsException("Credentials connectionString missing SubscriptionId");
            if (!dict.ContainsKey("ManagementCertificate"))
                throw new ConfigurationErrorsException(
                    "Credentials connectionString missing ManagementCertificate");
        }

        private static Dictionary<string, string> ParseConnectionString(string connectionString)
        {
            return connectionString.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries)
                .Where(slice => slice.Contains("="))
                .Select(slice => slice.Split(new[] {'='}, 2))
                .ToDictionary(split => split[0], split => split[1], StringComparer.OrdinalIgnoreCase);
        }

        private static string GetConnectionStringFromConfig()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Credentials"];
            if (connectionString == null)
                throw new ConfigurationErrorsException("Credentials connectionString not found.");
            return connectionString.ConnectionString;
        }
    }
}