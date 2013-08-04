using System.Collections.Generic;
using System.Xml;
using FluentAssertions;
using NUnit.Framework;

namespace SimpleConfig.Tests.Integration
{
    [TestFixture]
    public class Enumerable
    {

        const string SIMPLE = @"
<person>
  <pets>
    <pet>scruffy</pet>
    <pet>scratchy</pet>
    <pet>tiddles</pet>
  </pets>
</person>
";

        const string COMPLEX = @"
<person>
  <pets>
    <pet paws=""4"">
        <name>Cuddles</name>
    </pet>
    <pet paws=""3"">
        <name>Tiddles</name>
    </pet>
  </pets>
</person>
";

        [Test]
        public void CollectionsOfSimpleValuesShouldBeMapped()
        {
            var doc = new XmlDocument();
            doc.LoadXml(SIMPLE);

            var person = new ConfigMapper(doc.DocumentElement).GetObjectFromXml<SimplePersonRW>();
            var pets = person.Pets;
            pets.Count.Should().Be(3);
            pets[0].Should().Be("scruffy");
            pets[1].Should().Be("scratchy");
            pets[2].Should().Be("tiddles");
        }

        [Test]
        public void CollectionsOfSimpleValuesShouldBeMapped_EvenWhenThePropertyOnlyHasAGetter()
        {
            var doc = new XmlDocument();
            doc.LoadXml(SIMPLE);

            var person = new ConfigMapper(doc.DocumentElement).GetObjectFromXml<SimplePersonRO>();
            var pets = person.Pets;
            pets.Count.Should().Be(3);
            pets[0].Should().Be("scruffy");
            pets[1].Should().Be("scratchy");
            pets[2].Should().Be("tiddles");
        }

        [Test]
        public void CollectionsOfSimpleValuesShouldBeMapped_EvenWhenThePropertyOnlyHasASetter()
        {
            var doc = new XmlDocument();
            doc.LoadXml(SIMPLE);

            var person = new ConfigMapper(doc.DocumentElement).GetObjectFromXml<SimplePersonWO>();
            var pets = person.GetPets();
            pets.Count.Should().Be(3);
            pets[0].Should().Be("scruffy");
            pets[1].Should().Be("scratchy");
            pets[2].Should().Be("tiddles");
        }

        [Test]
        public void CollectionsOfComplexValuesShouldBeMapped()
        {
            var doc = new XmlDocument();
            doc.LoadXml(COMPLEX);

            var person = new ConfigMapper(doc.DocumentElement).GetObjectFromXml<ComplexPersonRW>();
            var pets = person.Pets;
            pets.Count.Should().Be(2);
            pets[0].Paws.Should().Be(4);
            pets[0].Name.Should().Be("Cuddles");
            pets[1].Paws.Should().Be(3);
            pets[1].Name.Should().Be("Tiddles");
        }

        [Test]
        public void CollectionsOfComplexValuesShouldBeMapped_EvenWhenThePropertyOnlyHasAGetter()
        {
            var doc = new XmlDocument();
            doc.LoadXml(COMPLEX);

            var person = new ConfigMapper(doc.DocumentElement).GetObjectFromXml<ComplexPersonRO>();
            var pets = person.Pets;
            pets.Count.Should().Be(2);
            pets[0].Paws.Should().Be(4);
            pets[0].Name.Should().Be("Cuddles");
            pets[1].Paws.Should().Be(3);
            pets[1].Name.Should().Be("Tiddles");
        }

        [Test]
        public void CollectionsOfComplexValuesShouldBeMapped_EvenWhenThePropertyOnlyHasASetter()
        {
            var doc = new XmlDocument();
            doc.LoadXml(COMPLEX);

            var person = new ConfigMapper(doc.DocumentElement).GetObjectFromXml<ComplexPersonWO>();
            var pets = person.GetPets();
            pets.Count.Should().Be(2);
            pets[0].Paws.Should().Be(4);
            pets[0].Name.Should().Be("Cuddles");
            pets[1].Paws.Should().Be(3);
            pets[1].Name.Should().Be("Tiddles");
        }

        public class SimplePersonRW
        {
            public List<string> Pets { get; set; }
        }

        public class ComplexPersonRW
        {
            public List<Pet> Pets { get; set; }
        }

        public class SimplePersonRO
        {
            private List<string> pets = new List<string>();
            public List<string> Pets
            {
                get { return pets; }
            }
        }

        public class ComplexPersonRO
        {
            private List<Pet> pets = new List<Pet>();
            public List<Pet> Pets
            {
                get { return pets; }
            }
        }

        public class SimplePersonWO
        {
            private List<string> pets;
            public List<string> Pets
            {
                set { pets = value; }
            }

            public List<string> GetPets()
            {
                return pets;
            }
        }

        public class ComplexPersonWO
        {
            private List<Pet> pets;
            public List<Pet> Pets
            {
                set { pets = value; }
            }

            public List<Pet> GetPets()
            {
                return pets;
            }
        }


        public class Pet
        {
            public string Name { get; set; }
            public int Paws { get; set; }
        }
    }

    
}
