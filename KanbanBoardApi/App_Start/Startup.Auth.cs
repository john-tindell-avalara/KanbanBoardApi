using System.Configuration;
using System.IdentityModel.Tokens;
using Microsoft.Owin.Security.ActiveDirectory;
using Microsoft.Owin.Security.Jwt;
using Owin;

namespace KanbanBoardApi
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            /*
            app.UseActiveDirectoryFederationServicesBearerAuthentication(new ActiveDirectoryFederationServicesBearerAuthenticationOptions
            {
                TokenValidationParameters =
                    new TokenValidationParameters() {ValidAudience = ConfigurationManager.AppSettings["ida:Audience"]}
            });
            */
            
            app.UseWindowsAzureActiveDirectoryBearerAuthentication(
                new WindowsAzureActiveDirectoryBearerAuthenticationOptions
                {
                    Audience = ConfigurationManager.AppSettings["ida:Audience"],
                    Tenant = ConfigurationManager.AppSettings["ida:Tenant"]
                });
                
                /*
            var audience = ConfigurationManager.AppSettings["ida:Audience"];

            app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
            {
                AllowedAudiences = new[] { audience },
            });
            */
        }
    }
}