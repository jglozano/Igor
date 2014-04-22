namespace Igor
{
    using System.Threading;
    using System.Threading.Tasks;
    using Annotations;
    using Helpers;
    using Models;
    using Simple.Web;
    using Simple.Web.Authentication;
    using Simple.Web.Behaviors;

    [UriTemplate("/websites/{WebSpace}/{SiteName}/operations/{OperationId}"), UsedImplicitly]
    public class GetOperationStatus : IGetAsync, IOutput<OperationStatusDto>, IRequireAuthentication
    {
        private readonly IClientHelper _clientHelper;

        public GetOperationStatus(IClientHelper clientHelper)
        {
            _clientHelper = clientHelper;
        }

        public async Task<Status> Get()
        {
            var client = _clientHelper.GetWebSiteClient();
            var status = await client.GetOperationStatusAsync(WebSpace, SiteName, OperationId, CancellationToken.None);
            Output = new OperationStatusDto
            {
                Status = status.Status.ToString()
            };
            return 200;
        }

        public string WebSpace { get; [UsedImplicitly] set; }
        public string SiteName { get; [UsedImplicitly] set; }
        public string OperationId { get; [UsedImplicitly] set; }
        public OperationStatusDto Output { get; private set; }
        public IUser CurrentUser { set; [UsedImplicitly] private get; }
    }
}