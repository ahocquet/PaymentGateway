using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PaymentGateway.Infrastructure.Json
{
    public class PrivateSetterContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(
            MemberInfo member,
            MemberSerialization memberSerialization)
        {
            var prop = base.CreateProperty(member, memberSerialization);

            if (prop.Writable)
            {
                return prop;
            }

            if (member is PropertyInfo property && property.GetSetMethod(true) != null)
            {
                prop.Writable = true;
            }

            return prop;
        }
    }
}
