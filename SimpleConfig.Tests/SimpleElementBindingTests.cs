using System.Xml;
using FluentAssertions;
using NUnit.Framework;

namespace SimpleConfig.Tests
{
    [TestFixture]
    public class SimpleElementBindingTests
    {
        [Test]
        public void AttributeStringPropertyBindingShouldBindStrings()
        {
            var xml = new XmlDocument();
            xml.LoadXml(@"<bob><Username>bob</Username></bob>");

            var configMapper = new ConfigMapper(xml.DocumentElement);
            var result = (User)configMapper.GetObjectFromXml(typeof(User));
            result.Username.Should().Be("bob");
        }

        [Test]
        public void AttributeStringPropertyBindingShouldBindInts()
        {
            var xml = new XmlDocument();
            xml.LoadXml(@"<bob><Age>3</Age></bob>");

            var configMapper = new ConfigMapper(xml.DocumentElement);
            var result = (User)configMapper.GetObjectFromXml(typeof(User));
            result.Age.Should().Be(3);
        }

        [Test]
        public void AttributeStringPropertyBindingShouldBindDecimals()
        {
            var xml = new XmlDocument();
            xml.LoadXml(@"<bob><Height>1.8</Height></bob>");

            var configMapper = new ConfigMapper(xml.DocumentElement);
            var result = (User)configMapper.GetObjectFromXml(typeof(User));
            result.Height.Should().Be(1.8m);
        }

        [Test]
        public void AttributeStringPropertyBindingShouldBindFloats()
        {
            var xml = new XmlDocument();
            xml.LoadXml(@"<bob><FavouriteNumber>1234567890123.45</FavouriteNumber></bob>");


            var configMapper = new ConfigMapper(xml.DocumentElement);
            var result = (User)configMapper.GetObjectFromXml(typeof(User));
            result.FavouriteNumber.Should().Be(1234567890123.45f);
        }

        [Test]
        public void AttributeBindingShouldBeCaseInsensitive()
        {
            var xml = new XmlDocument();
            xml.LoadXml(@"<bob><favouriteNUMBER>1234567890123.45</favouriteNUMBER></bob>");

            var configMapper = new ConfigMapper(xml.DocumentElement);
            var result = (User)configMapper.GetObjectFromXml(typeof(User));
            result.FavouriteNumber.Should().Be(1234567890123.45f);
        }

        public class User
        {
            public string Username { get; set; }
            public int Age { get; set; }
            public decimal Height { get; set; }
            public float FavouriteNumber { get; set; }
        }
    }
}


//public class SuperConfiguration
//{
//    public int NumberOfAttempts { get; private set; }

//    [ElementValue]
//    public string Key { get; private set; }

//    [AttributeValue("Async")]
//    public bool TryInAsyncMode { get; private set; }

//    public List<DefaultSimpleBindingTests.User> Users { get; private set; }
//}

/*
<settings name="bob" age="21">
  <Users>
    <bob username="bob" password="ilikecake">
  
    </bob>
    <neil username="bob">
        <password>cake<password>
    </neil>
  </Users>
 <settings>
*/