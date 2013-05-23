using System;
using System.Reflection;
using System.Xml;

namespace SimpleConfig.BindingStrategies
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CustomEnumerableAttribute : BaseBindingAttribute
    {
        public CustomEnumerableAttribute() { }
        public CustomEnumerableAttribute(string elementName)
        {
            ElementName = elementName;
        }

        public string ElementName { get; private set; }
        public Type ListType { get; private set; }

        public override IBindingStrategy MappingStrategy
        {
            get { return new EnumerableBindingStrategy(); }
        }
    }

    public class EnumerableBindingStrategy : IBindingStrategy
    {
        public bool Map(object destinationObject, PropertyInfo destinationProperty, XmlElement element, XmlElement allConfig, ConfigMapper mapper)
        {
            return true;
        }
    }
}