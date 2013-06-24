using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using NUnit.Framework;

namespace SimpleConfig.Tests.Integration
{
    [TestFixture]
    public class Enumerable
    {
        [Test]
        public void x()
        {
            var xml = @"
<person>
  <pets>
    <pet paws=""4"">
        <cuteName>Cuddles</cuteName>
    </pet>
    <pet paws=""3"">
        <cuteName>Tiddles</cuteName>
    </pet>
  </pets>
</person>
";

//             xml = @"
//<person age=""55"">
//  <Name>Poobarr</Name>
//</person>
//";

            var doc = new XmlDocument();
            doc.LoadXml(xml);

            var person = new ConfigMapper(doc.DocumentElement).GetObjectFromXml(typeof(Person));
        }
    }

    public class Person
    {
        public List<Pet> pets { get; set; }
        //public string Name { get; set; }
        //public int Age { get; set; }
    }

    public class Pet
    {
        public string CuteName { get; set; }
        public int Paws { get; set; }
    }
}
