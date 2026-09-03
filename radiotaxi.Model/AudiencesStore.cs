using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using authentication.Entities;
using authentication.Helpers;

namespace authentication.Models
{
    public static class AudiencesStore
    {

        public static Audience AddAudience(string name)
        {
            return null;
        }

        public static Audience FindAudience(string clientId)
        {
            Audience audience = null;
            authentication.Models.audience audi = Functions.GetAudience(clientId);
            if (audi != null)
            {
                audience.ClientId = audi.clientid;
                audience.Base64Secret = audi.secret;
                audience.Name = audi.name;
                return audience;
            }
            return null;
        }
    }
}