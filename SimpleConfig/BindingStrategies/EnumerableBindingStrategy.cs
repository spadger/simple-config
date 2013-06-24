using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml;
using SimpleConfig.Helpers;

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

        public override IBindingStrategy MappingStrategy
        {
            get { return new EnumerableBindingStrategy(ElementName); }
        }
    }

    public class EnumerableBindingStrategy : IBindingStrategy
    {

        public EnumerableBindingStrategy(){}
        public EnumerableBindingStrategy(string elementName)
        {
            ElementName = elementName;
        }

        public string ElementName { get; private set; }

        public bool Map(object destinationObject, PropertyInfo destinationProperty, XmlElement element, XmlElement allConfig, ConfigMapper mapper)
        {
            var collectionElement = element.GetElementNamed(ElementName ?? destinationProperty.Name);

            if (collectionElement == null)
            {
                return false;
            }

            object destinationCollection = GetDestinationCollection(destinationObject, destinationProperty);

            Action<object> adder = GetCollectionAdder(destinationCollection);
            var payloadType = destinationProperty.PropertyType.GetGenericArguments()[0];

            foreach (var childElement in collectionElement.ChildNodes.OfType<XmlElement>())
            {
                var mappedObject = mapper.GetObjectFromXml(payloadType, childElement);
                adder(mappedObject);
            }

            return true;
        }

        private object GetDestinationCollection(object destinationObject, PropertyInfo destinationProperty)
        {
            if (destinationProperty.CanRead && !destinationProperty.CanWrite)
            {
                return GetCollectionFromReadOnlyProperty(destinationObject, destinationProperty);
            }
            if (destinationProperty.CanWrite && !destinationProperty.CanRead)
            {
                return GetCollectionFromWriteOnlyProperty(destinationObject, destinationProperty);
            }

            if (destinationProperty.CanRead && destinationProperty.CanWrite)
            {
                return GetCollectionFromReadWriteProperty(destinationObject, destinationProperty);
            }
            
            throw new ConfigMappingException("Um, I found a property with no getter and no setter...");
        }

        private object GetCollectionFromReadOnlyProperty(object destinationObject, PropertyInfo destinationProperty)
        {
            var destinationCollection = destinationProperty.GetValue(destinationObject);
            if (destinationCollection == null)
            {
                throw new ConfigMappingException("Cannot populate property {0} of {1} because it is read-only and also null", destinationProperty.Name, destinationObject.GetType().AssemblyQualifiedName);
            }

            var destinationCollectionType = destinationCollection.GetType();
            if (!destinationCollectionType.IsAnInsertableSequence())
            {
                throw new ConfigMappingException("Cannot populate property {0} of {1} because it is read-only and its value is immutable (it is a {2})", destinationProperty.Name, destinationObject.GetType().AssemblyQualifiedName, destinationCollectionType.FullName);
            }
            return destinationCollection;
        }

        private object GetCollectionFromWriteOnlyProperty(object destinationObject, PropertyInfo destinationProperty)
        {
            var collection = CollectionFor(destinationProperty.PropertyType);
            destinationProperty.SetValue(destinationObject, collection);

            return collection;
        }

        private object GetCollectionFromReadWriteProperty(object destinationObject, PropertyInfo destinationProperty)
        {
            //if it's null, we will add one
            var destinationCollection = destinationProperty.GetValue(destinationObject);
            if (destinationCollection == null)
            {
                return GetCollectionFromWriteOnlyProperty(destinationObject, destinationProperty);
            }

            var destinationCollectionType = destinationCollection.GetType();
            //one exists, first, lets see if we can use the existing object
            if (!destinationCollectionType.IsAnInsertableSequence())
            {
                return GetCollectionFromWriteOnlyProperty(destinationObject, destinationProperty);
            }

            return destinationCollection;
        }

        private Action<object> GetCollectionAdder(object destinationCollection)
        {
            var destinationType = destinationCollection.GetType();

            //the specific generic collection type, e.g. typeof(ICollection<Person>)
            var typedICollectionType = destinationType.FindFirstImplementationOfGenericInterface(typeof (ICollection<>));

            var method = typedICollectionType.GetMethod("Add", BindingFlags.Instance | BindingFlags.Public);

            return
                x =>
                method.Invoke(destinationCollection, BindingFlags.Public | BindingFlags.Instance, null, new[] {x},
                              CultureInfo.CurrentCulture);
        }

        public object CollectionFor(Type type)
        {
            if (!type.IsGenericEnumerable())
            {
                throw new InvalidOperationException("Type does not implement IEnumerable<>: " + type.AssemblyQualifiedName);
            }

            if (type.CanBeInstantiated())
            {
                return type.Create();
            }

            if (type.IsInterface && type.IsGenericType)
            {
                var payloadType = type.GetGenericArguments()[0];
                var openListType = typeof (List<>);
                var specificType = openListType.MakeGenericType(payloadType);

                if (!specificType.IsA(type))
                {
                    throw new InvalidOperationException("Tried to use a generic List to represent a " + type.AssemblyQualifiedName + ", which did not work");
                }

                return specificType.Create();
            }
            
            if (!typeof (List<object>).IsA(type))
            {
                throw new InvalidOperationException("Tried to use a List<object> to represent a "  + type.AssemblyQualifiedName + ", which did not work");
            }
            return new List<object>();
        }
    }
}