using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace SimpleConfig.Helpers
{
    /// <summary>
    /// Creates a concrete stub for a given interface.  Borrows heavily from https://github.com/martinburrows/ConfigMapper
    /// </summary>
    public static class ConcreteTypeGenerator
    {
        private static readonly ConcurrentDictionary<Type, Type> typeCache = new ConcurrentDictionary<Type, Type>();

        public static object GetInstanceOf(Type t)
        {
            var concreteType = typeCache.GetOrAdd(t, GenerateConcreteFromInterface);

            var created = Activator.CreateInstance(concreteType);

            return created;
        }

        private static Type GenerateConcreteFromInterface(Type t)
        {
            ValidateRequestedType(t);

            var typeBuilder = GetTypeBuilder(t);

            Build(t, typeBuilder);

            var concreteType = typeBuilder.CreateType();

            return concreteType;
        }

        private static void Build(Type t, TypeBuilder typeBuilder)
        {
            CreateAutoPropertiesFor(t, typeBuilder);

            foreach (var @interface in t.GetInterfaces())
            {
                Build(@interface, typeBuilder);
            }
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
        }

        private static void CreateAutoPropertiesFor(Type interfaceType, TypeBuilder typeBuilder)
        {
            foreach (var property in interfaceType.GetProperties())
            {
                GenerateProperty(interfaceType, property, typeBuilder);
            }
        }

        private static void GenerateProperty(Type interfaceType, PropertyInfo interfaceProperty, TypeBuilder typeBuilder)
        {
            const MethodAttributes getSetVirtualAttributes = MethodAttributes.Public |
                                                             MethodAttributes.SpecialName |
                                                             MethodAttributes.HideBySig |
                                                             MethodAttributes.Virtual;

            var propertyType = interfaceProperty.PropertyType;

            var fieldBuilder = typeBuilder.DefineField(
                fieldName: "_" + interfaceProperty.Name,
                type: propertyType,
                attributes: FieldAttributes.Private);

            var propertyBuilder = typeBuilder.DefineProperty(
                name: interfaceProperty.Name,
                attributes: PropertyAttributes.HasDefault,
                returnType: propertyType,
                parameterTypes: Type.EmptyTypes);

            var getAccessorName = "get_" + interfaceProperty.Name;

            var getAccessorMethodBuilder = typeBuilder.DefineMethod(
                name: getAccessorName,
                attributes: getSetVirtualAttributes,
                returnType: propertyType,
                parameterTypes: Type.EmptyTypes);

            var getAccessorILGenerator = getAccessorMethodBuilder.GetILGenerator();
            getAccessorILGenerator.Emit(OpCodes.Ldarg_0);
            getAccessorILGenerator.Emit(OpCodes.Ldfld, fieldBuilder);
            getAccessorILGenerator.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getAccessorMethodBuilder);

            var setAccessorName = "set_" + interfaceProperty.Name;
            var setAccessorMethodBuiler = typeBuilder.DefineMethod(
                name: setAccessorName,
                attributes: getSetVirtualAttributes,
                returnType: null,
                parameterTypes: new[] { propertyType });

            var setAccessorILGenerator = setAccessorMethodBuiler.GetILGenerator();
            setAccessorILGenerator.Emit(OpCodes.Ldarg_0);
            setAccessorILGenerator.Emit(OpCodes.Ldarg_1);
            setAccessorILGenerator.Emit(OpCodes.Stfld, fieldBuilder);

            setAccessorILGenerator.Emit(OpCodes.Ret);
            propertyBuilder.SetSetMethod(setAccessorMethodBuiler);
        }

        /// <remarks>
        /// It would have been nice to create a single assembly & module
        /// but the test unit runner does not unload these things after each test
        /// </remarks>
        public static TypeBuilder GetTypeBuilder(Type interfaceType)
        {
            var assemblyName = new AssemblyName("SimpleConfig.Dynamic.InterfaceImplementations");
            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name);

            return moduleBuilder.DefineType(
                name: string.Format("SimpleConfig.Dynamic.InterfaceImplementations._{0}_Impl", interfaceType.FullName),
                attr: TypeAttributes.Public | TypeAttributes.Sealed,
                parent: typeof(object),
                interfaces: new[] { interfaceType });
        }
    }
}