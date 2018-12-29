using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using FluentAssertions;
using NUnit.Framework;
using SimpleConfig.BindingStrategies;
using SimpleConfig.Helpers;

namespace SimpleConfig.Tests.Helpers
{
    [TestFixture]
    public class PropertyHelperTests
    {
        private PropertyInfo PropertyNamed(string propertyName)
        {
            return typeof (SomeType).GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);
        }

        [TestCase("Int", false)]
        [TestCase("String", false)]
        [TestCase("Enum", false)]
        [TestCase("DateTime", false)]
        [TestCase("Class", true)]
        [TestCase("Interface", true)]
        [TestCase("Enumerable", true)]
        [TestCase("GenericEnumerable", true)]
        public void IsComplexType_ShouldDetermineWhetherAGivenPropertyIsComplex(string propertyName, bool isComplex)
        {
            PropertyNamed(propertyName).IsComplexType().Should().Be(isComplex);
        }

        [TestCase("Int", true)]
        [TestCase("NullableInt", true)]
        [TestCase("String", true)]
        [TestCase("Enum", true)]
        [TestCase("DateTime", true)]
        [TestCase("NullableDateTime", true)]
        [TestCase("Class", false)]
        [TestCase("Interface", false)]
        [TestCase("Enumerable", false)]
        [TestCase("GenericEnumerable", false)]
        public void IsForADirectlyPopulatableType_ShouldDetermineWhetherAGivenPropertyIsDirectlyPopulatable(string propertyName, bool isComplex)
        {
            PropertyNamed(propertyName).IsForADirectlyPopulatableType().Should().Be(isComplex);
        }

        [Test]
        public void GetMappingStrategies_ShouldBeAbleToFindACustomStrategy()
        {
            var strategies = PropertyNamed("CustomStrategy").GetMappingStrategies();
            strategies.Should().Contain(x => x is CustomBindingStrategy);
        }

        [Test]
        public void GetMappingStrategies_ShouldIgnoreNonStrategyAttributes()
        {
            var strategies = PropertyNamed("CustomStrategy").GetMappingStrategies();
            strategies.Should().HaveCount(1);
        }

        [Test]
        public void GetMappingStrategies_ShouldChooseComplexTypeBindingStrategyForComplexTypeProperties()
        {
            var strategies = PropertyNamed("Class").GetMappingStrategies();
            strategies.Should().HaveCount(1);
            strategies.Should().Contain(x => x is ComplexTypeBindingStrategy);
        }

        [TestCase("GenericEnumerable")]
        public void GetMappingStrategies_ShouldChooseEnumerableBindingStrategyForGenericEnumerableTypeProperties(string propertyName)
        {
            var strategies = PropertyNamed(propertyName).GetMappingStrategies();
            strategies.Should().HaveCount(1);
            strategies.Should().Contain(x => x is EnumerableBindingStrategy);
        }

        [TestCase("Enumerable")]
        [TestCase("IntArray")]
        [TestCase("IntArray")]
        [TestCase("ArrayList")]
        public void GetMappingStrategies_ShouldThrowAnExceptionForNonGenericEnumerableTypeProperties(string propertyName)
        {
            Action x = ()=>PropertyNamed(propertyName).GetMappingStrategies();
            x.Should().Throw<ConfigMappingException>();
        }

        [Test]
        public void GetMappingStrategies_ShouldChooseMultipleStartegiesForDirectlyPopulatableProperties()
        {
            var strategies = PropertyNamed("Int").GetMappingStrategies();
            strategies.Should().HaveCount(2);
            strategies.Should().Contain(x => x is AttributeValueMappingStrategy);
            strategies.Should().Contain(x => x is ElementValueMappingStrategy);
        }
    }

    public class SomeType
    {
        public int Int { get; set; }
        public int? NullableInt { get; set; }
        public string String { get; set; }
        public InnerEnum Enum { get; set; }
        public DateTime DateTime { get; set; }
        public DateTime? NullableDateTime { get; set; }
        public InnerClass Class { get; set; }
        public Interface Interface { get; set; }
        public IEnumerable Enumerable { get; set; }
        public IEnumerable<int> GenericEnumerable { get; set; }
        public int[] IntArray { get; set; }
        public List<int> IntList { get; set; }
        public ArrayList ArrayList { get; set; }

        [CLSCompliant(true)]
        [CustomBindingStrategy(typeof(CustomBindingStrategy))]
        public object CustomStrategy { get; set; }
        
    }

    public interface Interface{}
    public class InnerClass{}
    public enum InnerEnum{}

    public class CustomBindingStrategy : IBindingStrategy
    {
        public bool Map(object destinationObject, PropertyInfo destinationProperty, XmlElement element, XmlElement allConfig,
                        ConfigMapper mapper)
        {
            return true;
        }
    }
}
