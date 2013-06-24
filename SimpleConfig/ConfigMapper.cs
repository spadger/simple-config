using System;
using System.Linq;
using System.Reflection;
using System.Xml;
using SimpleConfig.Helpers;

namespace SimpleConfig
{
    public class ConfigMapper
    {
        private readonly XmlElement _configDocument;

        public ConfigMapper(XmlElement configDocument)
        {
            _configDocument = configDocument;
        }

        public object GetObjectFromXml(Type type)
        {
            return GetObjectFromXml(type, _configDocument);
        }

        public object GetObjectFromXml(Type type, XmlElement element)
        {
            var result = type.Create();
            PopulateObject(result, element);
            return result;
        }

        public void PopulateObject(object destination, XmlElement element)
        {
            var type = destination.GetType();
            
            foreach (var property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(x=>x.CanWrite || x.GetType().IsGenericEnumerable()))
            {
                PopulateProperty(destination, property, element);
            }
        }

        private void PopulateProperty(object destinationInstance, PropertyInfo destinationProperty, XmlElement element)
        {
            var mappingStrategies = destinationProperty.GetMappingStrategies();

            
            if (mappingStrategies==null)
            {
                throw new ConfigMappingException("It is not possible to map property {0} (of type {1}) because no mapping strategy could be found", destinationProperty.Name, destinationProperty.PropertyType.FullName);
            }

            foreach (var strategy in mappingStrategies)
            {
                if (strategy.Map(destinationInstance, destinationProperty, element, _configDocument, this))
                {
                    break;
                }
            }
        }
    }
}