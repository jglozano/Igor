namespace Igor.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class ConnectionStringParser
    {
        private static readonly char[] Semicolon = {';'};
        private static readonly char[] EqualSign = {'='};

        public static Dictionary<string, string> ParseConnectionString(string connectionString)
        {
            return connectionString.Split(Semicolon, StringSplitOptions.RemoveEmptyEntries)
                .Where(slice => slice.Contains("="))
                .Select(slice => slice.Split(EqualSign, 2))
                .ToDictionary(split => split[0], split => split[1], StringComparer.OrdinalIgnoreCase);
        }
    }
}