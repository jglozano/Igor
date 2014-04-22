namespace Igor
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.ConstrainedExecution;
    using System.Security.Policy;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.WindowsAzure.Management.WebSites.Models;
    using Simple.Web;
    using Simple.Web.Behaviors;

    [UriTemplate("/websites")]
    public class GetWebSites : IGetAsync, IOutput<DtoCollection<WebSiteDto>>
    {
        public async Task<Status> Get()
        {
            var sites = new SettingsHelper().GetSites().ToList();
            Trace.WriteLine("Sites: " + string.Join(", ", sites));
            var siteSet = new HashSet<string>(sites.Concat(sites.Select(s => s + "(staging)")), StringComparer.OrdinalIgnoreCase);

            WebSpacesListResponse spaces;
            try
            {
                var client = new ClientHelper().GetWebSiteClient();
                spaces = await client.WebSpaces.ListAsync(CancellationToken.None);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                throw;
            }
            try
            {
                var siteLists = await Task.WhenAll(spaces.Select(space => ListWebSites(space.Name, siteSet)));
                Output = DtoCollection.Create(siteLists.SelectMany(l => l).Select(WebSiteDto.FromSdk).OrderBy(dto => dto.Name));
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                throw;
            }
            return 200;
        }

        private static async Task<List<WebSite>>  ListWebSites(string webSpace, HashSet<string> siteSet)
        {
            var client = new ClientHelper().GetWebSiteClient();
            var parameters = new WebSiteListParameters
            {
                PropertiesToInclude = new[] {"Site.SiteProperties.Metadata", "Site.SiteProperties.Properties"}
            };
            var response = await client.WebSpaces.ListWebSitesAsync(webSpace, parameters, CancellationToken.None);
            return response
                .Where(s => siteSet.Contains(s.Name))
                .ToList();
        }

        public DtoCollection<WebSiteDto> Output { get; private set; }
    }
}