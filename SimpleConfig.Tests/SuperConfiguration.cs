using System.Collections.Generic;
using SimpleConfig.BindingStrategies;

namespace SimpleConfig.Tests
{
    public class SuperConfiguration
    {
        public int NumberOfAttempts { get; private set; }

        [ElementValue]
        public string Key { get; private set; }

        [AttributeValue("Async")]
        public bool TryInAsyncMode { get; private set; }

        public List<User> Users { get; private set; }
    }

    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}

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