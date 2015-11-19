using System;
using System.Xml;
using FluentAssertions;
using NUnit.Framework;

namespace SimpleConfig.Tests.BindingStrategies
{
    [TestFixture]
    public class SimpleElementBindingTests
    {
        private ConfigMapper ConfigMapperFor(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            return new ConfigMapper(doc.DocumentElement);
        }

        [Test]
        public void ElementBindingShouldBindStrings()
        {
            var configMapper = ConfigMapperFor(@"<bob><Username>bob</Username></bob>");

            var result = (User)configMapper.GetObjectFromXml(typeof(User));
            result.Username.Should().Be("bob");
        }

        [Test]
        public void ElementBindingShouldBindInts()
        {
            var configMapper = ConfigMapperFor(@"<bob><Age>3</Age></bob>");

            var result = (User)configMapper.GetObjectFromXml(typeof(User));
            result.Age.Should().Be(3);
        }

        [Test]
        public void ElementBindingShouldBindDecimals()
        {
            var configMapper = ConfigMapperFor(@"<bob><Height>1.8</Height></bob>");

            var result = (User)configMapper.GetObjectFromXml(typeof(User));
            result.Height.Should().Be(1.8m);
        }

        [Test]
        public void ElementBindingShouldBindFloats()
        {
            var configMapper = ConfigMapperFor(@"<bob><FavouriteNumber>1234567890123.45</FavouriteNumber></bob>");

            var result = (User)configMapper.GetObjectFromXml(typeof(User));
            result.FavouriteNumber.Should().Be(1234567890123.45f);
        }

        [Test]
        public void AttributeBindingShouldBindDateTimes()
        {
            var configMapper = ConfigMapperFor(@"<bob><DateOfBirth>03/Jul/1983</DateOfBirth></bob>");
            var result = (User)configMapper.GetObjectFromXml(typeof(User));
            result.DateOfBirth.Should().Be(new DateTime(1983, 7, 3));
        }

        [Test]
        public void AttributeBindingShouldBindNullableDateTimes()
        {
            var configMapper = ConfigMapperFor(@"<bob><Anniversary>11/Oct/2013</Anniversary></bob>");
            var result = (User)configMapper.GetObjectFromXml(typeof(User));
            result.Anniversary.Should().Be(new DateTime(2013, 10, 11));
        }

        [Test]
        public void AttributeBindingShouldBindEmptyNullableDateTimes()
        {
            var configMapper = ConfigMapperFor(@"<bob><Anniversary /></bob>");
            var result = (User)configMapper.GetObjectFromXml(typeof(User));
            result.Anniversary.Should().Be(null);
        }

        [Test]
        public void ElementBindingShouldBindEnumsByCastingToInts()
        {
            var configMapper = ConfigMapperFor(@"<bob><UserType>1</UserType></bob>");

            var result = (User)configMapper.GetObjectFromXml(typeof(User));
            result.UserType.Should().Be(UserType.Enhanced);
        }

        [Test]
        public void ElementBindingShouldBindEnumsByMatchingStrings()
        {
            var configMapper = ConfigMapperFor(@"<bob><UserType>Enhanced</UserType></bob>");

            var result = (User)configMapper.GetObjectFromXml(typeof(User));
            result.UserType.Should().Be(UserType.Enhanced);
        }

        [Test]
        public void ElementBindingShouldBeCaseInsensitive()
        {
            var configMapper = ConfigMapperFor(@"<bob><favouriteNUMBER>1234567890123.45</favouriteNUMBER></bob>");

            var result = (User)configMapper.GetObjectFromXml(typeof(User));
            result.FavouriteNumber.Should().Be(1234567890123.45f);
        }

        [Test]
        public void ElementBindingShouldBindTimespan()
        {
            var configMapper = ConfigMapperFor(@"<bob><timeLeft>00:10:00</timeLeft></bob>");
            var result = (User)configMapper.GetObjectFromXml(typeof(User));

            result.TimeLeft.Should().Be(TimeSpan.FromMinutes(10));
        }

        public class User
        {
            public string Username { get; set; }
            public int Age { get; set; }
            public decimal Height { get; set; }
            public float FavouriteNumber { get; set; }
            public DateTime DateOfBirth { get; set; }
            public DateTime? Anniversary { get; set; }
            public UserType UserType { get; set; }
            public TimeSpan TimeLeft { get; set; }
        }

        public enum UserType
        {
            Standard = 0,
            Enhanced = 1,
            Admin = 2

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