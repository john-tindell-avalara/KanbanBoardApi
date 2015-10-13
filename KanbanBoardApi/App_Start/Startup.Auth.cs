using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens;
using Microsoft.Owin.Security.ActiveDirectory;
using Microsoft.Owin.Security.DataHandler.Encoder;
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
            /*
            app.UseWindowsAzureActiveDirectoryBearerAuthentication(
                new WindowsAzureActiveDirectoryBearerAuthenticationOptions
                {
                    Audience = ConfigurationManager.AppSettings["ida:Audience"],
                    Tenant = ConfigurationManager.AppSettings["ida:Tenant"]
                });
                */

            var audience = "http://kanban.yeticode.co.uk/"; //ConfigurationManager.AppSettings["ida:Audience"];

            var symmetricKeyAsBase64 = "qMCdFDQuF23RV1Y-1Gq9L3cF3VmuFwVbam4fMTdAfpo";
            var secureKey = TextEncodings.Base64Url.Decode(symmetricKeyAsBase64);

            //var sSKey = new InMemorySymmetricSecurityKey(secureKey);
            //var signatureAlgorithm = "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256";
            //var digestAlgorithm = "http://www.w3.org/2001/04/xmlenc#sha256";

            app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
            {
                AllowedAudiences = new[] { audience },
                IssuerSecurityTokenProviders = new List<IIssuerSecurityTokenProvider>
                {
                    new SymmetricKeyIssuerSecurityTokenProvider("tpr", secureKey)
                }
            });
            
        }
    }
}