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

    [UriTemplate("/websites/{WebSpace}/{Name}/swap/{Slot}")]
    public class Swap : IPostAsync<WebSiteDto>, IOutput<SwapResultDto>, IRequireAuthentication
    {
        private readonly IClientHelper _clientHelper;

        public Swap(IClientHelper clientHelper)
        {
            _clientHelper = clientHelper;
        }

        public async Task<Status> Post(WebSiteDto site)
        {
            var client = _clientHelper.GetWebSiteClient();
            var operation = await client.WebSites.BeginSwapingSlotsAsync(WebSpace, Name, Slot, CancellationToken.None);
            Output = new SwapResultDto
            {
                OperationId = operation.OperationId,
                SiteName = Name,
                WebSpace = WebSpace
            };
            return 202;
        }

// ReSharper disable MemberCanBePrivate.Global
        public string WebSpace { get; [UsedImplicitly] set; }
        public string Name { get; [UsedImplicitly] set; }
        public string Slot { get; [UsedImplicitly] set; }
// ReSharper restore MemberCanBePrivate.Global
        public SwapResultDto Output { get; private set; }
        public IUser CurrentUser { set; [UsedImplicitly] private get; }
    }
}