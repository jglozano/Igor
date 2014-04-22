namespace Igor
{
    using Annotations;
    using Models;
    using Simple.Web;
    using Simple.Web.Authentication;
    using Simple.Web.Behaviors;

    [UriTemplate("/login"), UsedImplicitly]
    public class Login : IGet, IOutput<LoginResultDto>, IRequireAuthentication
    {
        public IUser CurrentUser { set; private get; }

        public Status Get()
        {
            Output = new LoginResultDto {Success = true};
            return Status.OK;
        }

        public LoginResultDto Output { get; private set; }
    }
}