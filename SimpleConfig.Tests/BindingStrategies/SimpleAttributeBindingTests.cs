using System.Xml;
using FluentAssertions;
using NUnit.Framework;

namespace SimpleConfig.Tests.BindingStrategies
{
    [TestFixture]
    public class SimpleAttributeBindingTests
    {
        [Test]
        public void AttributeBindingShouldBindStrings()
        {
            var xml = new XmlDocument();
            xml.LoadXml(@"<bob Username=""bob"" />");

            var configMapper = new ConfigMapper(xml.DocumentElement);
            var result = (User)configMapper.GetObjectFromXml(typeof(User));
            result.Username.Should().Be("bob");
        }

        [Test]
        public void AttributeBindingShouldBindInts()
        {
            var xml = new XmlDocument();
            xml.LoadXml(@"<bob Age=""3"" />");

            var configMapper = new ConfigMapper(xml.DocumentElement);
            var result = (User)configMapper.GetObjectFromXml(typeof(User));
            result.Age.Should().Be(3);
        }

        [Test]
        public void AttributeBindingShouldBindDecimals()
        {
            var xml = new XmlDocument();
            xml.LoadXml(@"<bob Height=""1.8"" />");

            var configMapper = new ConfigMapper(xml.DocumentElement);
            var result = (User)configMapper.GetObjectFromXml(typeof(User));
            result.Height.Should().Be(1.8m);
        }

        [Test]
        public void AttributeBindingShouldBindFloats()
        {
            var xml = new XmlDocument();
            xml.LoadXml(@"<bob FavouriteNumber=""1234567890123.45"" />");

            var configMapper = new ConfigMapper(xml.DocumentElement);
            var result = (User)configMapper.GetObjectFromXml(typeof(User));
            result.FavouriteNumber.Should().Be(1234567890123.45f);
        }

        [Test]
        public void AttributeBindingShouldBindEnumsByCastingToInts()
        {
            var xml = new XmlDocument();
            xml.LoadXml(@"<bob UserType=""1"" />");

            var configMapper = new ConfigMapper(xml.DocumentElement);
            var result = (User)configMapper.GetObjectFromXml(typeof(User));
            result.UserType.Should().Be(UserType.Enhanced);
        }

        [Test]
        public void AttributeBindingShouldBindEnumsByMatchingStrings()
        {
            var xml = new XmlDocument();
            xml.LoadXml(@"<bob UserType=""Enhanced"" />");

            var configMapper = new ConfigMapper(xml.DocumentElement);
            var result = (User)configMapper.GetObjectFromXml(typeof(User));
            result.UserType.Should().Be(UserType.Enhanced);
        }

        [Test]
        public void AttributeBindingShouldBeCaseInsensitive()
        {
            var xml = new XmlDocument();
            xml.LoadXml(@"<bob favouriteNUMBER=""1234567890123.45"" />");

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
            public UserType UserType { get; set; }
        }

        public enum UserType
        {
            Standard=0,
            Enhanced=1,
            Admin=2

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