using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SimpleConfig.BindingStrategies;

namespace SimpleConfig.Helpers
{
    public static class PropertyHelper
    {
        public static bool IsComplexType(this PropertyInfo @this)
        {
            return @this.PropertyType.IsComplex();
        }

        public static bool IsDirectlyPopulatable(this PropertyInfo @this)
        {
            return !@this.PropertyType.IsComplex();
        }

        public static IEnumerable<IBindingStrategy> GetMappingStrategies(this PropertyInfo @this)
        {
            var mappingStrategies = @this.GetCustomAttributes<BaseBindingAttribute>()
                                                .Select(x => x.MappingStrategy)
                                                .Where(x => x != null)
                                                .ToArray();

            if (mappingStrategies.Any())
            {
                return mappingStrategies;
            }

            if (@this.IsDirectlyPopulatable())
            {
                return new IBindingStrategy[] { new AttributeValueMappingStrategy(), new ElementValueMappingStrategy() };
            }

            if (@this.PropertyType.IsEnumerable())
            {
                return new[] { new EnumerableBindingStrategy() };
            }

            if (@this.IsComplexType())
            {
                return new[] { new EnumerableBindingStrategy() };
            }

            return null;
        }
    }
}