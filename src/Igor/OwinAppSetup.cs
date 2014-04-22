using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Simple.Web;

namespace Igor
{
    using Simple.Owin.Static;
    using UseAction = Action<Func<IDictionary<string, object>, Func<IDictionary<string, object>, Task>, Task>>;

    public class OwinAppSetup
    {
        public static void Setup(UseAction use)
        {
            use(Statics.AddFileAlias("/index.html", "/")
                .AddFolder("/scripts")
                .AddFolder("/content"));

            use(Application.Run);
        }
    }
}