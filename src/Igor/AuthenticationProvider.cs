namespace Igor
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Web;
    using Helpers;
    using Simple.Web.Authentication;
    using Simple.Web.Http;

    public class AuthenticationProvider : IAuthenticationProvider
    {
        private readonly IClientHelper _clientHelper;
        private readonly ISettingsHelper _settingsHelper;

        public AuthenticationProvider(IClientHelper clientHelper, ISettingsHelper settingsHelper)
        {
            _clientHelper = clientHelper;
            _settingsHelper = settingsHelper;
        }

        public IUser GetLoggedInUser(IContext context)
        {
            string[] header;
            if (!context.Request.Headers.TryGetValue("Authorization", out header)) return null;
            if (header == null || header.Length != 1) return null;
            var user = HttpRuntime.Cache.Get(header[0]) as IgorUser;
            if (user != null) return user;
            var dict = ConnectionStringParser.ParseConnectionString(header[0]);
            if (!(dict.ContainsKey("User") && dict.ContainsKey("Password"))) return null;
            user = new IgorUser(dict["User"], Uri.UnescapeDataString(dict["Password"]));
            var site = _settingsHelper.GetSites().First();
            var client = _clientHelper.GetWebSiteExtensionsClient(site, user.Name, user.Password);
            try
            {
                var task = client.Settings.ListAsync(CancellationToken.None);
                task.Wait();
                if (task.IsFaulted || task.IsCanceled) return null;
                HttpRuntime.Cache[header[0]] = user;
                return user;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void SetLoggedInUser(IContext context, IUser user)
        {
        }
    }

    public class IgorUser : IUser
    {
        public IgorUser(string name, string password)
        {
            Guid = Guid.Empty;
            IsAuthenticated = true;
            Name = name;
            Password = password;
        }
        public Guid Guid { get; private set; }
        public string Name { get; set; }
        public bool IsAuthenticated { get; private set; }
        public string User { get; set; }
        public string Password { get; set; }
    }
}