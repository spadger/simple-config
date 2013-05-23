using System;
using System.Reflection;
using System.Xml;
using SimpleConfig.Helpers;

namespace SimpleConfig.BindingStrategies
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ElementValueAttribute : BaseBindingAttribute
    {
        public ElementValueAttribute() { }
        public ElementValueAttribute(string elementName)
        {
            ElementName = elementName;
        }

        public string ElementName { get; private set; }

        public override IBindingStrategy MappingStrategy
        {
            get { return new ElementValueMappingStrategy(ElementName); }
        }
    }

    public class ElementValueMappingStrategy : IBindingStrategy
    {
        public ElementValueMappingStrategy(){}

        public ElementValueMappingStrategy(string elementName)
        {
            ElementName = elementName;
        }

        public string ElementName { get; private set; }


        public bool Map(object destinationObject, PropertyInfo destinationProperty, XmlElement element, XmlElement allConfig, ConfigMapper mapper)
        {
            var childElement = element.GetElementNamed(ElementName ?? destinationProperty.Name);

            if (childElement == null)
            {
                return false;
            }



            return false;
        }
    }
}