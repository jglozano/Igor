using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Simple.Web;

namespace Igor
{
    using Annotations;
    using Simple.Owin;
    using Simple.Owin.Static;
    using UseAction = Action<Func<IDictionary<string, object>, Func<IDictionary<string, object>, Task>, Task>>;

    [UsedImplicitly]
    public class OwinAppSetup
    {
        [UsedImplicitly]
        public static void Setup(UseAction use)
        {
            use(HttpsOnly.Create());
            use(Statics.AddFileAlias("/index.html", "/")
                .AddFolder("/scripts")
                .AddFolder("/content")
                .AddFolder("/html"));

            use(Application.Run);
        }
    }
}