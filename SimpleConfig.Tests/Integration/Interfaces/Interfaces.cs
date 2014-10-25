using System.Xml;
using FluentAssertions;
using NUnit.Framework;

namespace SimpleConfig.Tests.Integration.Interfaces
{
    [TestFixture]
    public class SimpleBindings
    {
        const string XML = @"
<person>
    <name>Jon</name>
    <age>30</age>
</person>
";

        [Test]
        public void FlatInterfaceWithGettersAndSettersShouldBeSupported()
        {
            var doc = new XmlDocument();
            doc.LoadXml(XML);

            var mapper = new ConfigMapper(doc.DocumentElement);

            IPerson person = (dynamic)mapper;
            person.Name.Should().Be("Jon");
            person.Age.Should().Be(30);
        }

        [Test]
        public void FlatInterfaceWithGettersButNoSettersShouldBeSupported()
        {
            var doc = new XmlDocument();
            doc.LoadXml(XML);

            var mapper = new ConfigMapper(doc.DocumentElement);

            IPersonGettersOnly person = (dynamic)mapper;
            person.Name.Should().Be("Jon");
            person.Age.Should().Be(30);
        }

        public interface IPerson
        {
            string Name { get; set; }
            int Age { get; set; }
        }

        public interface IPersonGettersOnly
        {
            string Name { get; }
            int Age { get; }
        }
    }
}