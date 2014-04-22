namespace Igor
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.WindowsAzure.WebSitesExtensions.Models;
    using Simple.Web;
    using Simple.Web.Behaviors;
    using Simple.Web.Links;

    [UriTemplate("/websites/{WebSpace}/{HostName}/deployment")]
    [LinksFrom(typeof(WebSiteDto), "/websites/{WebSpace}/{HostName}/deployment", Rel = "deployment")]
    public class GetDeployment : IGetAsync, IOutput<DeploymentDto>
    {
        public async Task<Status> Get()
        {
            var extensions = new ClientHelper().GetWebSiteExtensionsClient(HostName);
            try
            {
                var deployments = await extensions.Deployments.ListAsync(null, CancellationToken.None);
                var current = deployments.Deployments.FirstOrDefault(d => d.Active);
                if (current != null)
                {
                    Output = new DeploymentDto
                    {
                        Message = current.Message,
                        Time = current.ReceivedTime
                    };
                    return 200;
                }
                else
                {
                    return 404;
                }
            }
            catch
            {
                return 404;
            }
        }

        public string WebSpace { get; set; }
        public string HostName { get; set; }
        public DeploymentDto Output { get; private set; }
    }
}