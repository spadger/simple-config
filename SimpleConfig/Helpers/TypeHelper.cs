using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SimpleConfig.Helpers
{
    public static class TypeHelper
    {
        public static bool HasANoArgsConstructor(this Type @this)
        {
            return @this.GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                                .Any(x => x.GetParameters().Any() == false);
        }

        public static bool CanBeInstantiated(this Type @this)
        {
            return @this.IsInterface == false && @this.IsAbstract == false && @this.HasANoArgsConstructor();
        }

        public static object Create(this Type @this)
        {
            var constructor = @this.GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                                .FirstOrDefault(x => x.GetParameters().Any() == false);

            if (constructor == null)
            {
                throw new InvalidOperationException("Cannot instantiate the type " + @this.AssemblyQualifiedName + "because  no no-arg constructor could be found");
            }

            return constructor.Invoke(null);
        }

        public static bool IsComplex(this Type @this)
        {
            return @this.IsInterface || (@this.IsClass && @this != typeof(string));
        }

        public static bool IsA<T>(this Type @this)
        {
            return @this.IsA(typeof (T));
        }

        public static bool IsA(this Type @this, Type t)
        {
            return t.IsAssignableFrom(@this);
        }

        public static bool IsEnumerable(this Type @this)
        {
            return new[]
                {
                    typeof (IEnumerable),
                    typeof (IEnumerable<>)
                }.Any(x => x.IsAssignableFrom(@this)) && @this != typeof(string);
        }
    }
}