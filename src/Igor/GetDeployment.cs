namespace Igor
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Annotations;
    using Helpers;
    using Models;
    using Simple.Web;
    using Simple.Web.Authentication;
    using Simple.Web.Behaviors;
    using Simple.Web.Links;

    [UriTemplate("/websites/{WebSpace}/{HostName}/deployment")]
    [LinksFrom(typeof(WebSiteDto), "/websites/{WebSpace}/{HostName}/deployment", Rel = "deployment")]
    public class GetDeployment : IGetAsync, IOutput<DeploymentDto>, IRequireAuthentication
    {
        private readonly IClientHelper _clientHelper;

        public GetDeployment(IClientHelper clientHelper)
        {
            _clientHelper = clientHelper;
        }

        public async Task<Status> Get()
        {
            var user = (IgorUser) CurrentUser;
            var extensions = _clientHelper.GetWebSiteExtensionsClient(HostName, user.Name, user.Password);
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
                return 404;
            }
            catch
            {
                return 404;
            }
        }

        public string WebSpace { get; set; }
// ReSharper disable once MemberCanBePrivate.Global
        public string HostName { get; [UsedImplicitly] set; }
        public DeploymentDto Output { get; private set; }
        public IUser CurrentUser { set; private get; }
    }
}