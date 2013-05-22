using System;
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
            var result = type.Create();
            PopulateObject(result, _configDocument);
            return result;
        }

        private void PopulateObject(object destination, XmlElement element)
        {
            var type = destination.GetType();
            
            foreach (var property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (!property.CanWrite)
                {
                    continue;
                }

                PopulateProperty(property, element);
            }
        }

        private void PopulateProperty(PropertyInfo property, XmlElement element)
        {
            var type = property.PropertyType;

            if (type.CanBeInstantiated()==false)
            {
                throw new ConfigMappingException("It is not possible to create a new instance of {0} for the {1} property.", type.FullName, property.Name);
            }

            
        }
    }
}



/*
    private IObjectCreationStrategy GetObjectCreationStrategyFor(Type objectType)
    {
        if (objectType.IsPrimitive || objectType == typeof(string))
        {
            return null;
        }

        var customCreationStrategy = objectType.GetCustomAttributes<BaseObjectCreationAttribute>()
                                            .Select(x => x.ObjectCreationStrategy)
                                            .FirstOrDefault(x => x != null);

        if (customCreationStrategy != null)
        {
            return customCreationStrategy;
        }
            
        var hasDefaultConstructor = objectType.GetConstructors(BindingFlags.Instance | BindingFlags.Public)
                                                .Any(x => x.GetParameters().Any() == false);
            
        if (hasDefaultConstructor)
        {
            return new DefaultObjectCreationStrategy();
        }

        throw new ConfigMappingException("Could not instantiate a new instance of {0} because it does not have a default constructor, and the type has no [ObjectCreation] attribute.", objectType.FullName);
    }
*/