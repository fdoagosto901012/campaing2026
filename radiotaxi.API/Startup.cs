using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using Owin;
using Owin;
using radiotaxi.API.Providers;
using System;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Web;
using System.Web;
using System.Web.Http;
using System.Web.Http;

namespace radiotaxi.API
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            // Web API routes
            config.MapHttpAttributeRoutes();
            ConfigureOAuth(app);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
            app.MapSignalR();
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Newtonsoft.Json.Formatting.Indented,
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            };
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            var secret = Convert.ToString(ConfigurationManager.AppSettings["config:JwtKey"]);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Convert.ToString(ConfigurationManager.AppSettings["config:JwtKey"])));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(Convert.ToString(ConfigurationManager.AppSettings["config:JwtExpireDays"])));
            string issuerToken = Convert.ToString(ConfigurationManager.AppSettings["config:JwtIssuer"]);
            string audienceToken = Convert.ToString(ConfigurationManager.AppSettings["config:JwtAudience"]);
            
            var key = Encoding.ASCII.GetBytes(Convert.ToString(ConfigurationManager.AppSettings["config:JwtKey"]));
            app.UseJwtBearerAuthentication(
            new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    AllowedAudiences = new[] { audienceToken },
                    IssuerSecurityKeyProviders = new IIssuerSecurityKeyProvider[] {
                        new SymmetricKeyIssuerSecurityKeyProvider(issuerToken, key)
                    }
                }
            );
        }
    }
}