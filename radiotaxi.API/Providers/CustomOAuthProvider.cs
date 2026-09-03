using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace radiotaxi.API.Providers
{
    public class CustomOAuthProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId = string.Empty;
            string clientSecret = string.Empty;
            string symmetricKeyAsBase64 = string.Empty;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (context.ClientId == null)
            {
                context.SetError("invalid_clientId", "client_Id is not set");
                return Task.FromResult<object>(null);
            }

            string audience = null; // Functions.GetAudience(context.ClientId);

            if (audience == null)
            {
                context.SetError("invalid_clientId", string.Format("Invalid client_id '{0}'", context.ClientId));
                return Task.FromResult<object>(null);
            }

            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            //Dummy check here, you need to do your DB checks against membership system http://bit.ly/SPAAuthCode
            string userPassword = null; // Helpers.Functions.Hash(context.UserName + "_" + context.Password);
            if (context.UserName == null || context.Password == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect");
                //return;
                return Task.FromResult<object>(null);
            }

            string user = null; // Functions.GetUser(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect");
                //return;
                return Task.FromResult<object>(null);
            }

            if (true)//user.active == false
            {
                context.SetError("invalid_grant", "Contact the administrator");

                return Task.FromResult<object>(null);
            }


            if (true)//user.userRoles.Count() < 1
            {
                context.SetError("invalid_grant", "No role set");

                return Task.FromResult<object>(null);
            }


            if (true) //user.userDepartments.Count() < 1
            {
                context.SetError("invalid_grant", "No department set");
                return Task.FromResult<object>(null);
            }

            var identity = new ClaimsIdentity("JWT");

            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            //identity.AddClaim(new Claim("sub", context.UserName));
            //identity.AddClaim(new Claim("role", user.userRoles.FirstOrDefault().role.name));
            //identity.AddClaim(new Claim("Company", user.userCompanies.FirstOrDefault().company.name));
            //identity.AddClaim(new Claim("Department", user.userDepartments.FirstOrDefault().department.name));
            //identity.AddClaim(new Claim("DepartmentId", user.userDepartments.FirstOrDefault().department.id.ToString()));
            //identity.AddClaim(new Claim("Name", user.firstName));
            //identity.AddClaim(new Claim("LastName", user.lastName));
            //identity.AddClaim(new Claim("User", Newtonsoft.Json.JsonConvert.SerializeObject(user)));

            try
            {
                //Task.Run(() => Functions.SaveLastLogin(context.UserName));
            }
            catch
            {

            }
            var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    {
                         "audience", (context.ClientId == null) ? string.Empty : context.ClientId
                    }
                });
            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);
            return Task.FromResult<object>(null);
        }
    }
}