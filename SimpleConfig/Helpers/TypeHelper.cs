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

        public static object CreateFromObjectOrInterface(this Type @this)
        {
            if (@this.IsInterface)
            {
                return ConcreteTypeGenerator.GetInstanceOf(@this);
            }

            return @this.Create();
        }

        public static object Create(this Type @this)
        {
            var constructor = @this.GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                                .FirstOrDefault(x => x.GetParameters().Any() == false);

            if (constructor == null)
            {
                throw new InvalidOperationException("Cannot instantiate the type " + @this.AssemblyQualifiedName + "because no no-arg constructor could be found");
            }

            return constructor.Invoke(null);
        }

        public static bool IsComplex(this Type @this)
        {
            return( @this.IsInterface || (@this.IsClass && @this != typeof(string)));
        }

        public static bool IsPlainEnumerableButNotGenericEnumerable(this Type @this)
        {
            return @this != typeof(string) && @this.IsA<IEnumerable>() && !@this.IsGenericEnumerable();
        }

        public static bool IsA<T>(this Type @this)
        {
            return @this.IsA(typeof (T));
        }

        public static bool IsA(this Type @this, Type t)
        {
            return t.IsAssignableFrom(@this);
        }

        public static bool IsGenericEnumerable(this Type @this)
        {
            if (@this == typeof (string) || @this.IsA<Array>())
            {
                return false;
            }

            if (@this.IsGenericEnumerableInterface())
            {
                return true;
            }

            return @this.GetInterfaces().Any(iface => iface.IsGenericEnumerableInterface());
        }

        private static bool IsGenericEnumerableInterface(this Type @this)
        {
            return @this.IsInterface
                   && @this.IsGenericType
                   && @this.GetGenericTypeDefinition() == typeof (IEnumerable<>);
        }

        public static bool IsNullable(this Type @this)
        {
            return @this.IsGenericType
                   && @this.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static bool IsAnInsertableSequence(this Type @this)
        {
            if (@this.IsA<Array>())
            {
                return false;
            }
            if (@this.InterfaceIsAnInsertableSequence())
            {
                return true;
            }

            return @this.GetInterfaces().Any(iface => iface.InterfaceIsAnInsertableSequence());
        }

        public static bool InterfaceIsAnInsertableSequence(this Type @this)
        {
            var result = @this.IsInterface
                         && @this.IsGenericType
                         && @this.GetGenericTypeDefinition() == typeof (ICollection<>);

            return result;
        }

        public static Type FindFirstImplementationOfGenericInterface(this Type @this, Type requiredInterface)
        {
            AssertTypeIsAnOpenGenericInterface(requiredInterface);
            if (@this.ImplementsOpenGenericInterface(requiredInterface))
            {
                return @this;
            }

            return @this.GetInterfaces().FirstOrDefault(x=>x.ImplementsOpenGenericInterface(requiredInterface));
        }

        private static bool ImplementsOpenGenericInterface(this Type @this, Type requiredInterface)
        {
            //We know that T is an open generic

            if (!@this.IsGenericType)
            {
                return false;
            }

            //we passed in an open generic
            if (@this == requiredInterface)
            {
                return true;
            }

            return @this.GetGenericTypeDefinition() == requiredInterface;
        }

        private static void AssertTypeIsAnOpenGenericInterface(Type type)
        {
            if (type.IsInterface && type.IsGenericTypeDefinition)
            {
                return;
            }

            throw new ConfigMappingException("{0} is not an open generic interface", type.FullName);
        }
    }
}