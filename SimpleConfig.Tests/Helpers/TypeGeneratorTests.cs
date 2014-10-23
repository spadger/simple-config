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
            Action x = () => new TypeGenerator().ValidateRequestedType(typeof(object));
            x.ShouldThrow<ConfigMappingException>().WithMessage("requested type is not an interface");
        }

        [Test]
        public void ValidateRequestedType_WhenTargetInterfaceContainsAMethod_ShouldThrow()
        {
            Action x = () => new TypeGenerator().ValidateRequestedType(typeof(PropertyWithMethod));
            x.ShouldThrow<ConfigMappingException>().WithMessage("requested type may not have methods");
        }

        [Test]
        public void ValidateRequestedType_WhenTargetInterfaceContainsANonReadableProperty_ShouldThrow()
        {          
            Action x = () => new TypeGenerator().ValidateRequestedType(typeof(NoPublicGetter));
            x.ShouldThrow<ConfigMappingException>().WithMessage("write-only properties are not supported");
        }

        [Test]
        public void ValidateRequestedType_WhenTargetInterfaceContainsAWritableNoProperty_ShouldNotThrow()
        {
            new TypeGenerator().ValidateRequestedType(typeof(NoPublicSetter));
        }

        [Test]
        public void ValidateRequestedType_WhenTargetInterfaceContainsNoProperties_ShouldThrow()
        {
            Action x = () => new TypeGenerator().ValidateRequestedType(typeof(NoProperties));
            x.ShouldThrow<ConfigMappingException>().WithMessage("no properties were found");
        }
    }
}