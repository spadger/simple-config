using System.Xml;
using FluentAssertions;
using NUnit.Framework;

namespace SimpleConfig.Tests.Integration.Interfaces
{
    [TestFixture]
    public class CompoundInheritence
    {
        const string XML = @"
<employee>
    <id>A123</id>
    <name>Jon</name>
    <specialistField>Cheese</specialistField>
</employee>
";

        [Test]
        public void InterfaceWithSingleSubInterfaceShouldBeSupported()
        {
            var doc = new XmlDocument();
            doc.LoadXml(XML);

            var mapper = new ConfigMapper(doc.DocumentElement);

            IEmployee employee = (dynamic)mapper;
            employee.Id.Should().Be("A123");
            employee.Name.Should().Be("Jon");
        }

        [Test]
        public void InterfaceWithMultipleSubInterfacesShouldBeSupported()
        {
            var doc = new XmlDocument();
            doc.LoadXml(XML);

            var mapper = new ConfigMapper(doc.DocumentElement);

            ISpecialistEmployee employee = (dynamic)mapper;
            employee.Name.Should().Be("Jon");
            employee.SpecialistField.Should().Be("Cheese");
        }

        [Test]
        public void InterfaceWithMultipleConflictingInterfacesShouldBeSupported()
        {
            var doc = new XmlDocument();
            doc.LoadXml(XML);

            var mapper = new ConfigMapper(doc.DocumentElement);

            IDepartmentEmployee employee = (dynamic)mapper;
            ((IPerson)employee).Name.Should().Be("Jon");
            ((IHaveADepartment)employee).Name.Should().Be("Jon"); //example kinda sucks, but you are also somewhat insane
        }

        public interface IPerson
        {
            string Name { get; }
        }

        public interface IProductSpecialist
        {
            string SpecialistField { get; }
        }

        public interface IHaveADepartment
        {
            string Name { get; set; } 
        }

        public interface IEmployee : IPerson
        {
            string Id { get; set; }
        }

        public interface IDepartmentEmployee : IPerson, IHaveADepartment
        {
            string Id { get; set; }
        }

        public interface ISpecialistEmployee : IPerson, IProductSpecialist
        { }
    }
}