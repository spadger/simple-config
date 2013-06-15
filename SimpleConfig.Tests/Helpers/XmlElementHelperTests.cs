using System.Xml;
using FluentAssertions;
using NUnit.Framework;
using SimpleConfig.Helpers;

namespace SimpleConfig.Tests.Helpers
{
    [TestFixture]
    public class XmlElementHelperTests
    {
        private XmlElement ConfigMapperFor(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            return doc.DocumentElement;
        }

        [Test]
        public void GetAttributeValue_ReturnNullWhenNoMatchingAttributeExists()
        {
            ConfigMapperFor(@"<x b=""1"" />").GetAttributeValue("a").Should().BeNull();
        }

        [Test]
        public void GetAttributeValue_ReturnNullWhenNoAttributesExists()
        {
            ConfigMapperFor(@"<x />").GetAttributeValue("a").Should().BeNull();
        }

        [Test]
        public void GetAttributeValue_ReturnAttributeWhenMatchingAttributeExists()
        {
            ConfigMapperFor(@"<x a=""1"" />").GetAttributeValue("a").Should().Be("1");
        }

        [Test]
        public void GetAttributeValue_ReturnAttributeWhenMatchingAttributesExistsWithDifferentCase()
        {
            ConfigMapperFor(@"<x a=""1"" />").GetAttributeValue("A").Should().Be("1");
            ConfigMapperFor(@"<x A=""1"" />").GetAttributeValue("a").Should().Be("1");
        }

        [TestCase(@"<x a=""1"" A=""1"" />", "a", "1")]
        [TestCase(@"<x A=""1"" a=""1"" />", "a", "1")]
        public void GetAttributeValue_ReturnAttributeWhenMultipleMatchingAttributesExists(string xml, string requestedAttributeName, string expectedResult)
        {
            ConfigMapperFor(xml).GetAttributeValue(requestedAttributeName).Should().Be(expectedResult);
        }
        

        [Test]
        public void GetElementNamed_ReturnNullWhenNoMatchingElementExists()
        {
            ConfigMapperFor(@"<x><b /></x>").GetElementNamed("a").Should().BeNull();
        }

        [Test]
        public void GetElementNamed_ReturnNullWhenNoElementsExists()
        {
            ConfigMapperFor(@"<x />").GetElementNamed("a").Should().BeNull();
        }

        [Test]
        public void GetElementNamed_ReturnElementWhenMatchingElementExists()
        {
            var result = ConfigMapperFor(@"<x><a /></x>").GetElementNamed("a");
            result.Should().NotBeNull();
            result.Name.Should().Be("a");
        }

        [Test]
        public void GetElementNamed_ReturnElementWhenMatchingElementExistsWithDifferentCase()
        {
            var result = ConfigMapperFor(@"<x><A /></x>").GetElementNamed("a");
            result.Should().NotBeNull();
            result.Name.Should().Be("A");
        }

        [TestCase(@"<x><a /><a /></x>", "a", "a")]
        [TestCase(@"<x><a /><A /></x>", "a", "a")]
        [TestCase(@"<x><A /><a /></x>", "a", "A")]
        [TestCase(@"<x><A /><A /></x>", "a", "A")]
        public void GetElementNamed_ReturnElementWhenMultipleMatchingElementsExists(string xml, string requestedElementName, string expectedResult)
        {
            var element = ConfigMapperFor(xml).GetElementNamed(requestedElementName);
            element.Name.Should().Be(expectedResult);
        }
    }
}
