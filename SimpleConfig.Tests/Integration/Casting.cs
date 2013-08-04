using System.Xml;
using FluentAssertions;
using NUnit.Framework;

namespace SimpleConfig.Tests.Integration
{
    [TestFixture]
    public class Casting
    {
        const string XML = @"
<person>
    <name>Jon</name>
    <age>30</age>
</person>
";

        [Test]
        public void ExplicitCastingShouldInvokeTheMapper()
        {
            var doc = new XmlDocument();
            doc.LoadXml(XML);

            var mapper = new ConfigMapper(doc.DocumentElement);

            var person = (Person)(dynamic)mapper;
            person.Name.Should().Be("Jon");
            person.Age.Should().Be(30);
        }

        [Test]
        public void ImplicitCastingShouldInvokeTheMapper()
        {
            var doc = new XmlDocument();
            doc.LoadXml(XML);

            var mapper = new ConfigMapper(doc.DocumentElement);

            Person person = (dynamic)mapper;
            person.Name.Should().Be("Jon");
            person.Age.Should().Be(30);
        }

        
        public class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }
    }
}