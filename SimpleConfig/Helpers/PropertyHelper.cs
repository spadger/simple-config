using System;
using System.Collections;
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

        public static bool IsPlainEnumerableButNotGenericEnumerable(this PropertyInfo @this)
        {
            return @this.PropertyType.IsPlainEnumerableButNotGenericEnumerable();
        }

        public static bool IsForADirectlyPopulatableType(this PropertyInfo @this)
        {
            return !@this.PropertyType.IsComplex();
        }

        public static IEnumerable<IBindingStrategy> GetMappingStrategies(this PropertyInfo @this)
        {
            var customMappingStrategies = @this.GetCustomAttributes<BaseBindingAttribute>()
                                                .Select(x => x.MappingStrategy)
                                                .Where(x => x != null)
                                                .ToArray();

            if (customMappingStrategies.Any())
            {
                return customMappingStrategies;
            }

            if (@this.IsPlainEnumerableButNotGenericEnumerable())
            {
                throw new ConfigMappingException("{0} is a non-generic enumerable which is not supported because it is impossible to know what type to fill it with.", @this.PropertyType.AssemblyQualifiedName);
            }

            if (@this.IsForADirectlyPopulatableType())
            {
                return new IBindingStrategy[] { new AttributeValueMappingStrategy(), new ElementValueMappingStrategy() };
            }

            if (@this.PropertyType.IsGenericEnumerable())
            {
                return new[] { new EnumerableBindingStrategy() };
            }
            
            //This is our last chance, but also guaranteed to be complex due to the nature of IsDirectlyPopulatable
            return new[] { new ComplexTypeBindingStrategy() };
        }
    }
}