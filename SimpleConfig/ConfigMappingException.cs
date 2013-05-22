using System.Configuration;
using System.Xml;

namespace SimpleConfig
{
    public class ConfigMappingException : ConfigurationErrorsException
    {
        public ConfigMappingException()
        {}

        public ConfigMappingException(string message): base(message)
        { }

        public ConfigMappingException(string messageTemplate, params object[] tokens):this(string.Format(messageTemplate, tokens))
        { }

        public ConfigMappingException(string message, string filename, int line) : base(message, filename, line)
        {}

        public ConfigMappingException(string message, XmlNode node) : base(message, node)
        {}
    }
}