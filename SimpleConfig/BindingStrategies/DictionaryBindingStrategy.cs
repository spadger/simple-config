using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml;
using SimpleConfig.Helpers;

namespace SimpleConfig.BindingStrategies
{

    public enum DictionarySchemaItemLocationType
    {
        Attribute,
        SubElementName,
        SubElementValue
    }
    
    public class DictionaryXmlSchemaItemLocation
    {
        public DictionaryXmlSchemaItemLocation(DictionarySchemaItemLocationType locationType, string locationName)
        {
            LocationType = locationType;
            LocationName = locationName;
        }

        public DictionarySchemaItemLocationType LocationType { get; private set; }
        public string LocationName { get; private set; }
    }

    public class DictionaryXmlSchema
    {
        public DictionaryXmlSchema(DictionarySchemaItemLocationType keyLocationType, string keyLocationName, DictionarySchemaItemLocationType valueLocationType, string valueLocationName)
        {
            Key = new DictionaryXmlSchemaItemLocation(keyLocationType, keyLocationName);
            Value = new DictionaryXmlSchemaItemLocation(valueLocationType, valueLocationName);
        }

        public DictionaryXmlSchemaItemLocation Key { get; private set; }
        public DictionaryXmlSchemaItemLocation Value { get; private set; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class DictionaryAttribute : BaseBindingAttribute
    {
        public DictionaryAttribute(string elementName, DictionarySchemaItemLocationType keyLocationType, string keyLocationName, DictionarySchemaItemLocationType valueLocationType, string valueLocationName)
        {
            ElementName = elementName;
            DictionaryXmlSchema = new DictionaryXmlSchema(keyLocationType, keyLocationName, valueLocationType, valueLocationName);
        }

        public string ElementName { get; private set; }
        public DictionaryXmlSchema DictionaryXmlSchema { get; private set; }

        public override IBindingStrategy MappingStrategy
        {
            get { return new DictionaryBindingStrategy(ElementName, DictionaryXmlSchema); }
        }
    }

    public class DictionaryBindingStrategy : IBindingStrategy
    {
        public DictionaryBindingStrategy(string elementName, DictionaryXmlSchema dictionaryXmlSchema)
        {
            ElementName = elementName;
            DictionaryXmlSchema = dictionaryXmlSchema;
        }

        public string ElementName { get; private set; }
        public DictionaryXmlSchema DictionaryXmlSchema { get; set; }

        public bool Map(object parentObject, PropertyInfo destinationProperty, XmlElement element, XmlElement allConfig, ConfigMapper mapper)
        {
            throw new NotImplementedException();
            //var collectionElement = element.GetElementNamed(ElementName ?? destinationProperty.Name);
            //
            //if (collectionElement == null)
            //{
            //    return false;
            //}
            //
            //object destinationCollection = GetDestinationCollection(parentObject, destinationProperty);
            //
            //Action<object> adder = GetCollectionAdder(destinationCollection);
            //var payloadType = destinationProperty.PropertyType.GetGenericArguments()[0];
            //
            //if (payloadType.IsComplex())
            //{
            //    PopulateComplexValues(collectionElement, payloadType, mapper, adder);
            //}
            //else
            //{
            //    PopulateSimpleValues(collectionElement, payloadType, mapper, adder);
            //}
            //
            //return true;
        }

        //private void PopulateComplexValues(XmlElement collectionElement, Type payloadType, ConfigMapper mapper, Action<object> adder)
        //{
        //    foreach (var childElement in collectionElement.ChildNodes.OfType<XmlElement>())
        //    {
        //        var mappedObject = mapper.GetObjectFromXml(payloadType, childElement);
        //        adder(mappedObject);
        //    }
        //}

        //private void PopulateSimpleValues(XmlElement collectionElement, Type payloadType, ConfigMapper mapper, Action<object> adder)
        //{
        //    foreach (var childElement in collectionElement.ChildNodes.OfType<XmlElement>())
        //    {
        //        var elementValue = childElement.InnerText;

        //        if (payloadType.IsEnum)
        //        {
        //            var value = Enum.Parse(payloadType, elementValue);
        //            adder(value);
        //        }
        //        else if (payloadType.IsA<IConvertible>())
        //        {
        //            var value = Convert.ChangeType(elementValue, payloadType);
        //            adder(value);
        //        }
        //    }
        //}

        //private object GetDestinationCollection(object parentObject, PropertyInfo destinationProperty)
        //{
        //    if (destinationProperty.CanRead && !destinationProperty.CanWrite)
        //    {
        //        return GetCollectionFromReadOnlyProperty(parentObject, destinationProperty);
        //    }
        //    if (destinationProperty.CanWrite && !destinationProperty.CanRead)
        //    {
        //        return GetCollectionFromWriteOnlyProperty(parentObject, destinationProperty);
        //    }

        //    if (destinationProperty.CanRead && destinationProperty.CanWrite)
        //    {
        //        return GetCollectionFromReadWriteProperty(parentObject, destinationProperty);
        //    }
            
        //    throw new ConfigMappingException("Um, I found a property with no getter and no setter...");
        //}

        //private object GetCollectionFromReadOnlyProperty(object parentObject, PropertyInfo destinationProperty)
        //{
        //    var destinationCollection = destinationProperty.GetValue(parentObject, null);
        //    if (destinationCollection == null)
        //    {
        //        throw new ConfigMappingException("Cannot populate property {0} of {1} because it is read-only and also null", destinationProperty.Name, parentObject.GetType().AssemblyQualifiedName);
        //    }

        //    var destinationCollectionType = destinationCollection.GetType();
        //    if (!destinationCollectionType.IsAnInsertableSequence())
        //    {
        //        throw new ConfigMappingException("Cannot populate property {0} of {1} because it is read-only and its value is immutable (it is a {2})", destinationProperty.Name, parentObject.GetType().AssemblyQualifiedName, destinationCollectionType.FullName);
        //    }
        //    return destinationCollection;
        //}

        //private object GetCollectionFromWriteOnlyProperty(object parentObject, PropertyInfo destinationProperty)
        //{
        //    var collection = CollectionFor(destinationProperty.PropertyType);
        //    destinationProperty.SetValue(parentObject, collection, null);

        //    return collection;
        //}

        //private object GetCollectionFromReadWriteProperty(object parentObject, PropertyInfo destinationProperty)
        //{
        //    //if it's null, we will add one
        //    var destinationCollection = destinationProperty.GetValue(parentObject, null);
        //    if (destinationCollection == null)
        //    {
        //        return GetCollectionFromWriteOnlyProperty(parentObject, destinationProperty);
        //    }

        //    var destinationCollectionType = destinationCollection.GetType();
        //    //one exists, first, lets see if we can use the existing object
        //    if (!destinationCollectionType.IsAnInsertableSequence())
        //    {
        //        return GetCollectionFromWriteOnlyProperty(parentObject, destinationProperty);
        //    }

        //    return destinationCollection;
        //}

        //private Action<object> GetCollectionAdder(object destinationCollection)
        //{
        //    var destinationType = destinationCollection.GetType();

        //    //the specific generic collection type, e.g. typeof(ICollection<Person>)
        //    var typedICollectionType = destinationType.FindFirstImplementationOfGenericInterface(typeof (ICollection<>));

        //    var method = typedICollectionType.GetMethod("Add", BindingFlags.Instance | BindingFlags.Public);

        //    return
        //        x =>
        //        method.Invoke(destinationCollection, BindingFlags.Public | BindingFlags.Instance, null, new[] {x},
        //                      CultureInfo.CurrentCulture);
        //}

        //public object CollectionFor(Type type)
        //{
        //    if (!type.IsGenericEnumerable())
        //    {
        //        throw new InvalidOperationException("Type does not implement IEnumerable<>: " + type.AssemblyQualifiedName);
        //    }

        //    if (type.CanBeInstantiated())
        //    {
        //        return type.Create();
        //    }

        //    if (type.IsInterface && type.IsGenericType)
        //    {
        //        var payloadType = type.GetGenericArguments()[0];
        //        var openListType = typeof (List<>);
        //        var specificType = openListType.MakeGenericType(payloadType);

        //        if (!specificType.IsA(type))
        //        {
        //            throw new InvalidOperationException("Tried to use a generic List to represent a " + type.AssemblyQualifiedName + ", which did not work");
        //        }

        //        return specificType.Create();
        //    }
            
        //    if (!typeof (List<object>).IsA(type))
        //    {
        //        throw new InvalidOperationException("Tried to use a List<object> to represent a "  + type.AssemblyQualifiedName + ", which did not work");
        //    }
        //    return new List<object>();
        //}
    }
}