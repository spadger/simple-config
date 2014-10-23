using System;
using System.Linq;

namespace SimpleConfig.Helpers
{
    public class ConcreteTypeGenerator
    {
        public static object GenerateFromInterface(Type t)
        {
            ValidateRequestedType(t);

            return null;
        }

        public static void ValidateRequestedType(Type t)
        {
            if (!t.IsInterface)
            {
                throw new ConfigMappingException("requested type is not an interface");
            }

            if (t.GetMethods().Any(x=>!x.IsSpecialName))
            {
                throw new ConfigMappingException("requested type may not have methods");
            }

            if (t.GetProperties().Any(x => !x.CanRead))
            {
                throw new ConfigMappingException("write-only properties are not supported");
            }

            if (!t.GetProperties().Any())
            {
                throw new ConfigMappingException("no properties were found");
            }
        }
    }
}