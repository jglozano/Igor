namespace Igor
{
    using System.Linq;
    using Microsoft.WindowsAzure.Management.WebSites.Models;

    public class WebSiteDto
    {
        public string Name { get; set; }
        public string HostName { get; set; }

        public string ServerFarm { get; set; }

        public string WebSpace { get; set; }

        public static WebSiteDto FromSdk(WebSite webSite)
        {
            var hostName = webSite.HostNames.FirstOrDefault();
            if (hostName != null && hostName.Contains("."))
            {
                hostName = hostName.Substring(0, hostName.IndexOf('.'));
            }
            return new WebSiteDto
            {
                Name = webSite.Name,
                HostName = hostName,
                WebSpace = webSite.WebSpace,
                ServerFarm = webSite.ServerFarm
            };
        }
    }
}