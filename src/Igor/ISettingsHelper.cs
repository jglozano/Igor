namespace Igor
{
    using System.Collections.Generic;

    public interface ISettingsHelper
    {
        IEnumerable<string> GetSites();
    }
}