using System.Configuration;
using System.Xml;

namespace SimpleConfig
{
    public class SimpleConfigHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            //Yep, it's really as simple as that....
            return new ConfigMapper((XmlElement)section);
        }
    }
}