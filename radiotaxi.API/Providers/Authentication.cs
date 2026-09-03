using Microsoft.IdentityModel.Tokens;
using radiotaxi.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace radiotaxi.API.Providers
{
    public class Authentication
    {
        public static string GenerateJWTAuthetication(userlog user ,string jsonUser, string userName, string role)
        {
            try
            {
                var claims = new List<Claim>
                {
                    new Claim(JwtHeaderParameterNames.Jku, userName),
                    new Claim(JwtHeaderParameterNames.Kid, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, userName)
                };

                claims.Add(new Claim(ClaimTypes.Role, role));
                claims.Add(new Claim("username", userName));
                claims.Add(new Claim("firstName", user.firstName));
                claims.Add(new Claim("lastName", user.lastName));
                claims.Add(new Claim("email", user.email));
                claims.Add(new Claim("partnerReference", user.partnerReference == null ? "" : user.partnerReference));
                claims.Add(new Claim("phone", user.phone));
                claims.Add(new Claim("userId", user.userId.ToString()));
                claims.Add(new Claim("padronId", (user.padronID == null ? 0 : user.padronID).ToString()));
                claims.Add(new Claim("User", jsonUser));



                string JwtKey = Convert.ToString(ConfigurationManager.AppSettings["config:JwtKey"]);
                string JwtIssuer = Convert.ToString(ConfigurationManager.AppSettings["config:JwtIssuer"]);
                string JwtAudience = Convert.ToString(ConfigurationManager.AppSettings["config:JwtAudience"]);

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expires = DateTime.Now.AddDays(Convert.ToDouble(Convert.ToString(ConfigurationManager.AppSettings["config:JwtExpireDays"])));

                var token = new JwtSecurityToken(
                    JwtIssuer,
                    JwtAudience,
                    claims,
                    expires: expires,
                    signingCredentials: creds
                );
                string _token = new JwtSecurityTokenHandler().WriteToken(token);
                string response = Authentication.ValidateToken(_token);
                return _token;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public static string ValidateToken(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Convert.ToString(ConfigurationManager.AppSettings["config:JwtKey"]));
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);
                // Corrected access to the validatedToken
                var jwtToken = (JwtSecurityToken)validatedToken;
                var jku = jwtToken.Claims.First(claim => claim.Type == "jku").Value;
                var userName = jwtToken.Claims.First(claim => claim.Type == "kid").Value;
                return userName;
            }
            catch
            {
                return null;
            }
        }
    }
}