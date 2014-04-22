namespace Igor
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.WindowsAzure.Management.WebSites.Models;
    using Simple.Web;
    using Simple.Web.Behaviors;

    [UriTemplate("/websites/{WebSpace}/{SiteName}/operations/{OperationId}")]
    public class GetOperationStatus : IGetAsync, IOutput<OperationStatusDto>
    {
        public async Task<Status> Get()
        {
            var client = new ClientHelper().GetWebSiteClient();
            var status = await client.GetOperationStatusAsync(WebSpace, SiteName, OperationId, CancellationToken.None);
            Output = new OperationStatusDto
            {
                Status = status.Status.ToString()
            };
            return 200;
        }

        public string WebSpace { get; set; }
        public string SiteName { get; set; }
        public string OperationId { get; set; }
        public OperationStatusDto Output { get; private set; }
    }

    public class OperationStatusDto
    {
        public string Status { get; set; }
    }
}