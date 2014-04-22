namespace Igor
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Annotations;
    using Helpers;
    using Microsoft.WindowsAzure.Management.WebSites.Models;
    using Models;
    using Simple.Web;
    using Simple.Web.Authentication;
    using Simple.Web.Behaviors;

    [UriTemplate("/websites")]
    public class GetWebSites : IGetAsync, IOutput<DtoCollection<WebSiteDto>>, IRequireAuthentication
    {
        private readonly IClientHelper _clientHelper;
        private readonly ISettingsHelper _settingsHelper;

        public GetWebSites(IClientHelper clientHelper, ISettingsHelper settingsHelper)
        {
            _clientHelper = clientHelper;
            _settingsHelper = settingsHelper;
        }

        public async Task<Status> Get()
        {
            var sites = _settingsHelper.GetSites().ToList();
            Trace.WriteLine("Sites: " + string.Join(", ", sites));
            var siteSet = new HashSet<string>(sites.Concat(sites.Select(s => s + "(staging)")), StringComparer.OrdinalIgnoreCase);

            WebSpacesListResponse spaces;
            try
            {
                var client = _clientHelper.GetWebSiteClient();
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

        private async Task<List<WebSite>>  ListWebSites(string webSpace, HashSet<string> siteSet)
        {
            var client = _clientHelper.GetWebSiteClient();
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
        public IUser CurrentUser { set; [UsedImplicitly] private get; }
    }
}