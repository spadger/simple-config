using System;
using System.Xml;

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
            get { return new AttributeValueMappingStrategy(); }
        }
    }

    public class AttributeValueMappingStrategy : IBindingStrategy
    {
        public bool Do<T>(T destination, XmlElement element, XmlElement allConfig)
        {
            throw new NotImplementedException();
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class CustomBindingStrtategyAttribute : BaseBindingAttribute
    {
        public CustomBindingStrtategyAttribute(Type mappingStrategyType)
        {
            this.mappingStrategyType = mappingStrategyType;
        }

        private Type mappingStrategyType;

        public override IBindingStrategy MappingStrategy
        {
            get
            {
                var result = Activator.CreateInstance(mappingStrategyType) as IBindingStrategy;
                if (result ==null)
                {
                    throw new ConfigMappingException("Custom mapping strategy is not a mpaping strategy: {0}", mappingStrategyType.FullName);
                }
                return result;
            }
        }
    }
}