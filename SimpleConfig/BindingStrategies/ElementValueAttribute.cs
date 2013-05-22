using System;
using System.Xml;

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
            get { throw new NotImplementedException(); }
        }
    }



    public class ElementValueMappingStrategy : IBindingStrategy
    {
        public bool Do<T>(T destination, XmlElement element, XmlElement allConfig)
        {
            throw new NotImplementedException();
        }
    }
}