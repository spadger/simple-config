using System;
using System.Collections;
using System.Collections.Generic;
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

        [TestCase(typeof(Public_NoObviousConstructor), true)]
        [TestCase(typeof(Public_PublicConstructorWithArgs), false)]
        [TestCase(typeof(SomeInterface), false)]
        [TestCase(typeof(SomeAbstractClass), false)]
        public void CanBeInstantiated_ShouldDetermineWhetherGivenTypesCanBeInstantiatedUsingTheActivator(Type type, bool canBeInstantiated)
        {
            type.CanBeInstantiated().Should().Be(canBeInstantiated);
        }

        [TestCase(typeof(IEnumerable), true)]
        [TestCase(typeof(IEnumerable<string>), true)]
        [TestCase(typeof(ICollection<string>), true)]
        [TestCase(typeof(List<string>), true)]
        [TestCase(typeof(ArrayList), true)]
        [TestCase(typeof(string[]), true)]
        [TestCase(typeof(string[]), true)]
        [TestCase(typeof(string), false)]
        [TestCase(typeof(TypeHelperTests), false)]
        [TestCase(typeof(SomeEnum), false)]
        public void IsEnumerable_ShouldDetermineWhetherGivenTypesIsEnumerable(Type type, bool isEnumerable)
        {
            type.IsEnumerable().Should().Be(isEnumerable);
        }

        [TestCase(typeof(string), false)]
        [TestCase(typeof(int), false)]
        [TestCase(typeof(SomeEnum), false)]
        [TestCase(typeof(Person), true)]
        public void IsComplex_ShouldDetermineWhetherGivenTypesIsComplex(Type type, bool isComplex)
        {
            type.IsComplex().Should().Be(isComplex);
        }
    }

    public enum SomeEnum
    {
        A=0
    }

    public class Person{}
}