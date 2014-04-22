namespace Igor
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    public class SettingsHelper : ISettingsHelper
    {
        public IEnumerable<string> GetSites()
        {
            return ConfigurationManager.AppSettings["WebSites"].Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}