namespace Igor.Models
{
    using Annotations;

    public class SwapResultDto
    {
        public string SiteName { [UsedImplicitly] get; set; }
        public string WebSpace { [UsedImplicitly] get; set; }
        public string OperationId { [UsedImplicitly] get; set; }
    }
}