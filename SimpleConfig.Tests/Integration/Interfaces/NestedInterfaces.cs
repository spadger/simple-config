using System.Xml;
using FluentAssertions;
using NUnit.Framework;

namespace SimpleConfig.Tests.Integration.Interfaces
{
    [TestFixture]
    public class NestedInterfaces
    {
        const string XML = @"
<person>
    <school name=""westcliff"" />
</person>
";

        [Test]
        public void InterfaceFlatInterfaceWithGettersAndSettersShouldBeSupported()
        {
            var doc = new XmlDocument();
            doc.LoadXml(XML);

            var mapper = new ConfigMapper(doc.DocumentElement);

            IPerson person = (dynamic)mapper;
            
            var school = person.School;
            school.Should().NotBeNull();
            school.Name.Should().Be("westcliff");
        }

        [Test]
        public void InterfaceFlatInterfaceWithGettersButNoSettersShouldBeSupported()
        {
            var doc = new XmlDocument();
            doc.LoadXml(XML);

            var mapper = new ConfigMapper(doc.DocumentElement);

            IPersonGetSchoolOnly person = (dynamic)mapper;
            var school = person.School;
            school.Should().NotBeNull();
            school.Name.Should().Be("westcliff");
        }

        public interface IPerson
        {
            ISchool School { get; set; }
        }

        public interface IPersonGetSchoolOnly
        {
            ISchool School { get; }
        }

        public interface ISchool
        {
            string Name { get; }
        }
    }
}