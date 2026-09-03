using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace radiotaxi.WEB.Helper
{
    public class FirstLevelContractResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            // Solo incluir propiedades cuyos valores sean tipos primitivos o string
            return base.CreateProperties(type, memberSerialization)
                .Where(p =>
                    p.PropertyType.IsPrimitive ||
                    p.PropertyType == typeof(string) ||
                    p.PropertyType == typeof(DateTime) ||
                    p.PropertyType.IsEnum ||
                    p.PropertyType == typeof(decimal)
                ).ToList();
        }
    }
}