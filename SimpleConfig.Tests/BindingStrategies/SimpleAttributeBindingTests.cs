using System.Xml;
using FluentAssertions;
using NUnit.Framework;

namespace SimpleConfig.Tests.BindingStrategies
{
    [TestFixture]
    public class SimpleAttributeBindingTests
    {
        private ConfigMapper ConfigMapperFor(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            return new ConfigMapper(doc.DocumentElement);
        }

        [Test]
        public void AttributeBindingShouldBindStrings()
        {
            var configMapper = ConfigMapperFor(@"<bob Username=""bob"" />");
            var result = (User)configMapper.GetObjectFromXml(typeof(User));
            result.Username.Should().Be("bob");
        }

        [Test]
        public void AttributeBindingShouldBindInts()
        {
            var configMapper = ConfigMapperFor(@"<bob Age=""3"" />");

            var result = (User)configMapper.GetObjectFromXml(typeof(User));
            result.Age.Should().Be(3);
        }

        [Test]
        public void AttributeBindingShouldBindDecimals()
        {
            var configMapper = ConfigMapperFor(@"<bob Height=""1.8"" />");
            var result = (User)configMapper.GetObjectFromXml(typeof(User));
            result.Height.Should().Be(1.8m);
        }

        [Test]
        public void AttributeBindingShouldBindFloats()
        {
            var configMapper = ConfigMapperFor(@"<bob FavouriteNumber=""1234567890123.45"" />");
            var result = (User)configMapper.GetObjectFromXml(typeof(User));
            result.FavouriteNumber.Should().Be(1234567890123.45f);
        }

        [Test]
        public void AttributeBindingShouldBindEnumsByCastingToInts()
        {
            var configMapper = ConfigMapperFor(@"<bob UserType=""1"" />");

            var result = (User)configMapper.GetObjectFromXml(typeof(User));
            result.UserType.Should().Be(UserType.Enhanced);
        }

        [Test]
        public void AttributeBindingShouldBindEnumsByMatchingStrings()
        {
            var configMapper = ConfigMapperFor(@"<bob UserType=""Enhanced"" />");

            var result = (User)configMapper.GetObjectFromXml(typeof(User));
            result.UserType.Should().Be(UserType.Enhanced);
        }

        [Test]
        public void AttributeBindingShouldBeCaseInsensitive()
        {
            var configMapper = ConfigMapperFor(@"<bob favouriteNUMBER=""1234567890123.45"" />");

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