using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SimpleConfig.Tests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Pisser()
        {
            var xxxx = (Person)(dynamic) GetConfig();
            

          
        }

        private object GetConfig()
        {
            return new ConfigProvider();
        }
    }


    public class ConfigProvider : DynamicObject
    {
        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            result = new Person();
            return true;
        }
    }

    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
