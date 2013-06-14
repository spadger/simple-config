using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SimpleConfig.Helpers
{
    public static class TypeHelper
    {
        public static bool HasDefaultConstructor(this Type @this)
        {
            return @this.GetConstructors().Any(x => x.GetParameters().Any() == false && x.IsPublic);
        }

        public static bool CanBeInstantiated(this Type @this)
        {
            return @this.IsInterface == false && @this.IsAbstract == false && @this.HasDefaultConstructor();
        }

        public static object Create(this Type type)
        {
            return Activator.CreateInstance(type);
        }

        public static bool IsComplex(this Type @this)
        {
            return @this.IsClass && @this != typeof(string);
        }

        public static bool IsEnumerable(this Type @this)
        {
            return new[]
                {
                    typeof (IEnumerable),
                    typeof (IEnumerable<>)
                }.Any(x => x.IsAssignableFrom(@this));
        }
    }
}