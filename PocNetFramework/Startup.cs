using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.DataProtection;
using Newtonsoft.Json.Serialization;
using Owin;
using System.Web.Http;

[assembly: OwinStartup(typeof(PocNetFramework.Startup))]
namespace PocNetFramework
{
    public partial class Startup
    {
        public static IDataProtectionProvider DataProtectionProvider { get; private set; }
        public void Configuration(IAppBuilder app)
        {
            DataProtectionProvider = app.GetDataProtectionProvider();
            //UnityConfig.Provider = DataProtectionProvider;
            //ConfigureAuth(app);

            var config = new HttpConfiguration();
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;
            //config.Filters.Add(new ExceptionFilter(UnityConfig.Container.Resolve<ILogBusiness>(), UnityConfig.Container.Resolve<ISistemaBusiness>()));
            ConfigureOauth(app);

            //config.DependencyResolver = new UnityResolver(UnityConfig.Container);

            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls12;

            WebApiConfig.Register(config);
            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(config);
        }

        private static void ConfigureOauth(IAppBuilder app)
        {
            app.UseIdentityServerBearerTokenAuthentication(new IdentityServer3.AccessTokenValidation.IdentityServerBearerTokenAuthenticationOptions()
            {
                Authority = "https://localhost:5001",
                RequiredScopes = new [] {"api"}
            });
            //app.CreatePerOwinContext(GerenciadorContext.Create);
            //app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

        }
    }
}