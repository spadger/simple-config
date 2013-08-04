using System.Xml;
using FluentAssertions;
using NUnit.Framework;

namespace SimpleConfig.Tests.Integration
{
    [TestFixture]
    public class Complex
    {

        const string XML = @"
<person>
    <car>
        <make>Ford</make>
        <year>2010</year>
        <stereo>
            <model>Alpine</model>
            <mp3>true</mp3>
        </stereo>
    </car>
</person>
";

        [Test]
        public void CollectionsOfSimpleValuesShouldBeMapped()
        {
            var doc = new XmlDocument();
            doc.LoadXml(XML);

            var person = new ConfigMapper(doc.DocumentElement).GetObjectFromXml<Person>();
            var car = person.Car;
            car.Make.Should().Be("Ford");
            car.Year.Should().Be(2010);

            var stereo = car.Stereo;
            stereo.Model.Should().Be("Alpine");
            stereo.MP3.Should().BeTrue();
        }

        [Test]
        public void CollectionsOfSimpleValuesShouldBeMapped_EvenWhenThePropertyOnlyHasAGetter()
        {
            var doc = new XmlDocument();
            doc.LoadXml(XML);

            var person = new ConfigMapper(doc.DocumentElement).GetObjectFromXml<PersonRO>();
            var car = person.Car;
            car.Make.Should().Be("Ford");
            car.Year.Should().Be(2010);

            var stereo = car.Stereo;
            stereo.Model.Should().Be("Alpine");
            stereo.MP3.Should().BeTrue();
        }

        [Test]
        public void CollectionsOfSimpleValuesShouldBeMapped_EvenWhenThePropertyOnlyHasASetter()
        {
            var doc = new XmlDocument();
            doc.LoadXml(XML);

            var person = new ConfigMapper(doc.DocumentElement).GetObjectFromXml<PersonWO>();
            var car = person.GetCar();
            car.Make.Should().Be("Ford");
            car.Year.Should().Be(2010);

            var stereo = car.GetStereo();
            stereo.Model.Should().Be("Alpine");
            stereo.MP3.Should().BeTrue();
        }

        public class Person
        {
            public string Name { get; set; }
            public Car Car { get; set; }
        }

        public class Car
        {
            public string Make { get; set; }
            public int Year { get; set; }
            public Stereo Stereo { get; set; }
        }

        public class PersonRO
        {
            public string Name { get; set; }
            public CarRO car = new CarRO();
            public CarRO Car
            {
                get { return car; }
            }
        }

        public class CarRO
        {
            public string Make { get; set; }
            public int Year { get; set; }

            private Stereo stereo = new Stereo();
            public Stereo Stereo
            {
                get { return stereo; }

            }
        }

        public class PersonWO
        {
            public string Name { get; set; }
            private CarWO car;
            public CarWO Car
            {
                set { car = value; }
            }
            public CarWO GetCar()
            {
                return car;
            }
        }

        public class CarWO
        {

            public string Make { get; set; }
            public int Year { get; set; }

            private Stereo stereo;
            public Stereo Stereo
            {
                set { stereo = value; }
            }
            public Stereo GetStereo()
            {
                return stereo;
            }
        }

        public class Stereo
        {
            public string Model { get; set; }
            public bool MP3 { get; set; }
        }
    }
}
