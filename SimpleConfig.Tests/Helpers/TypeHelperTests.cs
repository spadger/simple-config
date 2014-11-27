using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

using SimpleConfig.Helpers;
using SimpleConfig.Tests.TestTypes;

namespace SimpleConfig.Tests.Helpers
{
    [TestFixture]
    public class TypeHelperTests
    {
        [TestCase("Public_NoObviousConstructor", true)]
        [TestCase("Public_PrivateNoArgConstructor", true)]
        [TestCase("Public_ProtectedNoArgConstructor", true)]
        [TestCase("Public_InternalNoArgConstructor", true)]
        [TestCase("Public_PublicConstructorWithArgs", false)]
        [TestCase("Public_PrivateConstructorWithArgs", false)]
        [TestCase("Public_ProtectedConstructorWithArgs", false)]
        [TestCase("Public_InternalConstructorWithArgs", false)]
        [TestCase("Internal_NoObviousConstructor", true)]
        [TestCase("Internal_PrivateNoArgConstructor", true)]
        [TestCase("Internal_ProtectedNoArgConstructor", true)]
        [TestCase("Internal_InternalNoArgConstructor", true)]
        [TestCase("Internal_PublicConstructorWithArgs", false)]
        [TestCase("Internal_PrivateConstructorWithArgs", false)]
        [TestCase("Internal_ProtectedConstructorWithArgs", false)]
        [TestCase("Internal_InternalConstructorWithArgs", false)]
        public void HasANoArgsConstructor_ShouldBeAbleTooDetermineIfATyeHasANoArgsConstructor(string typeName, bool expectedToHaveDefaultConstructor)
        {
            var type = Type.GetType("SimpleConfig.Tests.TestTypes." + typeName + ",SimpleConfig.Tests.TestTypes");
            type.HasANoArgsConstructor().Should().Be(expectedToHaveDefaultConstructor);
        }

        private const string TYPE_FORMAT = "SimpleConfig.Tests.TestTypes.{0},SimpleConfig.Tests.TestTypes";

        [TestCase("Public_NoObviousConstructor")]
        [TestCase("Public_PrivateNoArgConstructor")]
        [TestCase("Public_ProtectedNoArgConstructor")]
        [TestCase("Public_InternalNoArgConstructor")]
        [TestCase("Internal_NoObviousConstructor")]
        [TestCase("Internal_PrivateNoArgConstructor")]
        [TestCase("Internal_ProtectedNoArgConstructor")]
        [TestCase("Internal_InternalNoArgConstructor")]
        public void Create_ShouldBeAbleToCreateANewInstance(string classTypeName)
        {
            var classType = Type.GetType(string.Format(TYPE_FORMAT, classTypeName));
            var result = classType.Create();
            result.Should().NotBeNull();
        }

        [TestCase("Public_NoObviousConstructor")]
        [TestCase("Public_PrivateNoArgConstructor")]
        [TestCase("Public_ProtectedNoArgConstructor")]
        [TestCase("Public_InternalNoArgConstructor")]
        [TestCase("Internal_NoObviousConstructor")]
        [TestCase("Internal_PrivateNoArgConstructor")]
        [TestCase("Internal_ProtectedNoArgConstructor")]
        [TestCase("Internal_InternalNoArgConstructor")]
        public void Create_ShouldCreateANewInstanceOfTheCorrectType(string classTypeName)
        {
            var classType = Type.GetType(string.Format(TYPE_FORMAT, classTypeName));
            var result = classType.Create();
            result.GetType().Should().Be(classType);
        }

        [TestCase("Public_NoObviousConstructor")]
        [TestCase("SomeInterface")]
        public void CreateFromObjectOrInterface_ShouldCreateANewInstanceOfTheCorrectType(string classTypeName)
        {
            var classType = Type.GetType(string.Format(TYPE_FORMAT, classTypeName));
            var result = classType.CreateFromObjectOrInterface();
            classType.IsAssignableFrom(result.GetType()).Should().BeTrue();
        }

        [TestCase(typeof(Public_NoObviousConstructor), true)]
        [TestCase(typeof(Public_PublicConstructorWithArgs), false)]
        [TestCase(typeof(SomeInterfaces), false)]
        [TestCase(typeof(SomeAbstractClass), false)]
        public void CanBeInstantiated_ShouldDetermineWhetherGivenTypesCanBeInstantiatedUsingTheActivator(Type type, bool canBeInstantiated)
        {
            type.CanBeInstantiated().Should().Be(canBeInstantiated);
        }

        [TestCase(typeof(IEnumerable), false)]
        [TestCase(typeof(IEnumerable<string>), true)]
        [TestCase(typeof(ICollection<string>), true)]
        [TestCase(typeof(List<string>), true)]
        [TestCase(typeof(ArrayList), false)]
        [TestCase(typeof(string[]), false)]
        [TestCase(typeof(string), false)]
        [TestCase(typeof(TypeHelperTests), false)]
        [TestCase(typeof(SomeEnum), false)]
        public void IsEnumerable_ShouldDetermineWhetherGivenTypesIsEnumerable(Type type, bool isGenericEnumerable)
        {
            type.IsGenericEnumerable().Should().Be(isGenericEnumerable);
        }

        [TestCase(typeof(IEnumerable), false)]
        [TestCase(typeof(IEnumerable<string>), false)]
        [TestCase(typeof(ICollection<string>), true)]
        [TestCase(typeof(List<string>), true)]
        [TestCase(typeof(ArrayList), false)]
        [TestCase(typeof(string[]), false)]
        [TestCase(typeof(string), false)]
        [TestCase(typeof(TypeHelperTests), false)]
        [TestCase(typeof(SomeEnum), false)]
        public void IsAnInsertableCollection_ShouldDetermineWhetherGivenTypesIsAnInsertableCollection(Type type, bool isInsertable)
        {
            type.IsAnInsertableSequence().Should().Be(isInsertable);
        }

        
        [TestCase(typeof(string), false)]
        [TestCase(typeof(int), false)]
        [TestCase(typeof(SomeEnum), false)]
        [TestCase(typeof(Person), true)]
        public void IsComplex_ShouldDetermineWhetherGivenTypesIsComplex(Type type, bool isComplex)
        {
            type.IsComplex().Should().Be(isComplex);
        }

        [TestCase(typeof(List<string>), typeof(IEnumerable), true)]
        [TestCase(typeof(List<string>), typeof(List<string>), true)]
        [TestCase(typeof(IEnumerable), typeof(List<string>), false)]
        [TestCase(typeof(IEnumerable), typeof(IEnumerable), true)]
        [TestCase(typeof(List<string>), typeof(ArrayList), false)]
        public void IsA_ShouldDetermineWhetherGivenTypesIsAssignableFromAKnownType(Type typeA, Type typeB, bool isAssignable)
        {
            typeA.IsA(typeB).Should().Be(isAssignable);
        }

        [TestCase(typeof(List<string>), typeof(IEnumerable), true)]
        [TestCase(typeof(List<string>), typeof(List<string>), true)]
        [TestCase(typeof(IEnumerable), typeof(List<string>), false)]
        [TestCase(typeof(IEnumerable), typeof(IEnumerable), true)]
        [TestCase(typeof(List<string>), typeof(ArrayList), false)]
        public void IsA_Generic_ShouldDetermineWhetherGivenTypesIsAssignableFromAKnownType(Type typeA, Type typeB, bool isAssignable)
        {
            var method = typeof(TypeHelper).GetMethods().Where(x=>x.Name=="IsA").First(x=>x.IsGenericMethod);
            var openMethod = method.MakeGenericMethod(typeB);
            var result = (bool)openMethod.Invoke(null, new[] { typeA });

            result.Should().Be(isAssignable);
        }

        [TestCase(typeof(Nullable<int>), true)]
        [TestCase(typeof(int?), true)]
        [TestCase(typeof(int), false)]
        [TestCase(typeof(Nullable<DateTime>), true)]
        [TestCase(typeof(DateTime?), true)]
        [TestCase(typeof(DateTime), false)]
        [TestCase(typeof(SomeEnum?), true)]
        [TestCase(typeof(SomeEnum), false)]
        [TestCase(typeof(string), false)]
        public void IsNullable_ShouldCorrectlyIdentifyNullableTypes(Type type, bool isNullable)
        {
            type.IsNullable().Should().Be(isNullable);
        }
    }

    public enum SomeEnum
    {
        A=0
    }

    public class Person{}
}