using System;
using System.Reflection;
using System.Xml;
using SimpleConfig.Helpers;

namespace SimpleConfig.BindingStrategies
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AttributeValueAttribute : BaseBindingAttribute
    {
        public AttributeValueAttribute() { }
        public AttributeValueAttribute(string attributeName)
        {
            AttributeName = attributeName;
        }

        public string AttributeName { get; private set; }

        public override IBindingStrategy MappingStrategy
        {
            get { return new AttributeValueMappingStrategy(AttributeName); }
        }
    }

    public class AttributeValueMappingStrategy : IBindingStrategy
    {
        public AttributeValueMappingStrategy(){}

        public AttributeValueMappingStrategy(string attributeName)
        {
            AttributeName = attributeName;
        }

        public string AttributeName { get; private set; }

        public bool Map(object destinationObject, PropertyInfo destinationProperty, XmlElement element, XmlElement allConfig, ConfigMapper mapper)
        {
            var attributeValue = element.GetAttributeValue(AttributeName ??  destinationProperty.Name);

            if (attributeValue == null)
            {
                return false;
            }

            var destinationPropertyType = destinationProperty.PropertyType;

            if (destinationPropertyType.IsEnum)
            {
                var value = Enum.Parse(destinationPropertyType, attributeValue);
                destinationProperty.SetValue(destinationObject, value);
                return true;
            }

            if (typeof(IConvertible).IsAssignableFrom(destinationPropertyType))
            {
                var value = Convert.ChangeType(attributeValue, destinationPropertyType);
                destinationProperty.SetValue(destinationObject, value);
                return true;
            }

            return false;
        }
    }
}