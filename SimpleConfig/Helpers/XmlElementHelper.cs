using System;
using System.Linq;
using System.Xml;

namespace SimpleConfig.Helpers
{
    public static class XmlElementHelper
    {
        public static string GetAttributeValue(this XmlElement @this, string attributeName)
        {
            var result = @this.Attributes
                              .Cast<XmlAttribute>()
                              .FirstOrDefault(x => x.Name.Equals(attributeName, StringComparison.OrdinalIgnoreCase));

            return result == null ? null : result.Value;
        }

        public static XmlElement GetElementNamed(this XmlElement @this, string elementName)
        {
            var result = @this.ChildNodes
                              .OfType<XmlElement>()
                              .FirstOrDefault(x => x.Name.Equals(elementName, StringComparison.OrdinalIgnoreCase));

            return result;
        }
    }
}