using System.Configuration;
using System.Xml;

namespace SimpleConfig
{
    public class ConfigMappingException : ConfigurationErrorsException
    {
        public ConfigMappingException()
        {}

        public ConfigMappingException(string messageTemplate, params object[] tokens):base(string.Format(messageTemplate, tokens))
        { }

        public ConfigMappingException(string message, string filename, int line) : base(message, filename, line)
        {}

        public ConfigMappingException(XmlNode node, string messageTemplate, params object[] tokens): base(string.Format(messageTemplate, tokens), node)
        {}
    }
}