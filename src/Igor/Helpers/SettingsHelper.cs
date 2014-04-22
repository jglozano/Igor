namespace Igor.Helpers
{
    using System;
    using System.Collections.Generic;
    using Annotations;
    using Microsoft.WindowsAzure;

    [UsedImplicitly]
    public class SettingsHelper : ISettingsHelper
    {
        public IEnumerable<string> GetSites()
        {
            return CloudConfigurationManager.GetSetting("WebSites").Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}