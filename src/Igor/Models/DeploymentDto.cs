namespace Igor.Models
{
    using System;
    using Annotations;

    public class DeploymentDto
    {
        public string Message { [UsedImplicitly] get; set; }
        public DateTime Time { [UsedImplicitly] get; set; }
    }
}