using System;
using FluentAssertions;
using NUnit.Framework;
using SimpleConfig.Helpers;
using SimpleConfig.Tests.TestTypes;

namespace SimpleConfig.Tests.Helpers
{
    [TestFixture]
    public class TypeGeneratorTests
    {
        [Test]
        public void ValidateRequestedType_WhenTargetIsNotAnInterface_ShouldThrow()
        {
            Action x = () => ConcreteTypeGenerator.ValidateRequestedType(typeof(object));
            x.Should().Throw<ConfigMappingException>().WithMessage("requested type is not an interface");
        }

        [Test]
        public void ValidateRequestedType_WhenTargetInterfaceContainsAMethod_ShouldThrow()
        {
            Action x = () => ConcreteTypeGenerator.ValidateRequestedType(typeof(WithMethod));
            x.Should().Throw<ConfigMappingException>().WithMessage("requested type may not have methods");
        }

        [Test]
        public void ValidateRequestedType_WhenTargetInterfaceContainsANonReadableProperty_ShouldThrow()
        {
            Action x = () => ConcreteTypeGenerator.ValidateRequestedType(typeof(NoGetter));
            x.Should().Throw<ConfigMappingException>().WithMessage("write-only properties are not supported");
        }

        [Test]
        public void ValidateRequestedType_WhenTargetInterfaceContainsAWritableNoProperty_ShouldNotThrow()
        {
            ConcreteTypeGenerator.ValidateRequestedType(typeof(NoSetter));
        }

        [Test]
        public void GetTypeBuilder_WhenConcreteTypeIsCreated_ShouldCreateReasonableClassName()
        {
            var builder = ConcreteTypeGenerator.GetTypeBuilder(typeof(SomeBindableInterface));
            builder.FullName.Should().Be("SimpleConfig.Dynamic.InterfaceImplementations._SimpleConfig.Tests.TestTypes.SomeBindableInterface_Impl");
        }

        [Test]
        public void GetTypeBuilder_WhenConcreteTypeIsCreated_ShouldCreateAReasonableAssemblyName()
        {
            var builder = ConcreteTypeGenerator.GetTypeBuilder(typeof(SomeBindableInterface));
            builder.Assembly.FullName.Should().StartWith("SimpleConfig.Dynamic.InterfaceImplementations");
        }

        [Test]
        public void GenerateFromInterface_BaseInterfaceHasAGetterAndSetter_ShouldYieldAWorkingType()
        {
            var concrete = (GetterAndSetter)ConcreteTypeGenerator.GetInstanceOf(typeof(GetterAndSetter));
            concrete.X = 123;
            concrete.X.Should().Be(123);

            concrete.X = 456;
            concrete.X.Should().Be(456);
        }

        [Test]
        public void GenerateFromInterface_BaseInterfaceOnlyHasAGetter_ShouldYieldAWorkingType()
        {
            var concrete = (NoSetter)ConcreteTypeGenerator.GetInstanceOf(typeof(NoSetter));
            concrete.X.Should().Be(0);
            concrete.GetType().GetProperties()[0].SetValue(concrete, 123, new object[0]);
            concrete.X.Should().Be(123);
        }
    }
}