namespace Igor.Models
{
    using System.Linq;
    using Annotations;
    using Microsoft.WindowsAzure.Management.WebSites.Models;

    public class WebSiteDto
    {
// ReSharper disable once MemberCanBePrivate.Global
        public string Name { get; set; }
// ReSharper disable MemberCanBePrivate.Global
        public string HostName { [UsedImplicitly] get; set; }

        public string ServerFarm { [UsedImplicitly] get; set; }

        public string WebSpace { [UsedImplicitly] get; set; }
// ReSharper restore MemberCanBePrivate.Global

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