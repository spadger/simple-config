using System.Reflection;
using System.Xml;
using SimpleConfig.Helpers;

namespace SimpleConfig.BindingStrategies
{
    public class ComplexTypeBindingStrategy : IBindingStrategy
    {
        public ComplexTypeBindingStrategy(){}
        public ComplexTypeBindingStrategy(string elementName)
        {
            ElementName = elementName;
        }

        private string ElementName { get; set; }

        public bool Map(object parentObject, PropertyInfo destinationProperty, XmlElement element, XmlElement allConfig, ConfigMapper mapper)
        {
            var objectElement = element.GetElementNamed(ElementName ?? destinationProperty.Name);

            if (objectElement == null)
            {
                return false;
            }

            object destinationObject = GetDestinationObject(parentObject, destinationProperty);

            /////////not sure about this....
            mapper.PopulateObject(destinationObject, objectElement);

            return true;
        }

        private object GetDestinationObject(object parentObject, PropertyInfo destinationProperty)
        {
            if (destinationProperty.CanRead && !destinationProperty.CanWrite)
            {
                return GetObjectFromReadOnlyProperty(parentObject, destinationProperty);
            }
            if (destinationProperty.CanWrite && !destinationProperty.CanRead)
            {
                return GetObjectFromWriteOnlyProperty(parentObject, destinationProperty);
            }

            if (destinationProperty.CanRead && destinationProperty.CanWrite)
            {
                return GetObjectFromReadWriteProperty(parentObject, destinationProperty);
            }

            throw new ConfigMappingException("Um, I found a property with no getter and no setter...");
        }

        private object GetObjectFromReadOnlyProperty(object parentObject, PropertyInfo destinationProperty)
        {
            var destinationObject = destinationProperty.GetValue(parentObject);
            if (destinationObject == null)
            {
                throw new ConfigMappingException("Cannot populate property {0} of {1} because it is read-only and also null", destinationProperty.Name, destinationObject.GetType().AssemblyQualifiedName);
            }

            return destinationObject;
        }

        private object GetObjectFromWriteOnlyProperty(object parentObject, PropertyInfo destinationProperty)
        {
            var destinationObject = destinationProperty.PropertyType.Create();
            destinationProperty.SetValue(parentObject, destinationObject);

            return destinationObject;
        }

        private object GetObjectFromReadWriteProperty(object parentObject, PropertyInfo destinationProperty)
        {
            //if it's null, we will add one
            var destinationObject = destinationProperty.GetValue(parentObject);
            if (destinationObject != null)
            {
                return destinationObject;
            }

            return GetObjectFromWriteOnlyProperty(parentObject, destinationProperty);
        }
    }
}