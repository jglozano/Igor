namespace Igor
{
    using System.Threading;
    using System.Threading.Tasks;
    using Simple.Web;
    using Simple.Web.Behaviors;

    [UriTemplate("/websites/{WebSpace}/{Name}/swap/{Slot}")]
    public class Swap : IPostAsync<WebSiteDto>, IOutput<SwapResultDto>
    {
        public async Task<Status> Post(WebSiteDto site)
        {
            var client = new ClientHelper().GetWebSiteClient();
            var operation = await client.WebSites.BeginSwapingSlotsAsync(WebSpace, Name, Slot, CancellationToken.None);
            Output = new SwapResultDto
            {
                OperationId = operation.OperationId,
                SiteName = Name,
                WebSpace = WebSpace
            };
            return 202;
        }

        public string WebSpace { get; set; }
        public string Name { get; set; }
        public string Slot { get; set; }
        public SwapResultDto Output { get; private set; }
    }
}