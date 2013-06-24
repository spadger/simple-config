using System;

namespace SimpleConfig.BindingStrategies
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CustomBindingStrategyAttribute : BaseBindingAttribute
    {
        public CustomBindingStrategyAttribute(Type mappingStrategyType)
        {
            MappingStrategyType = mappingStrategyType;
        }

        public Type MappingStrategyType { get; private set; }

        public override IBindingStrategy MappingStrategy
        {
            get
            {
                var result = Activator.CreateInstance(MappingStrategyType) as IBindingStrategy;
                if (result == null)
                {
                    throw new ConfigMappingException("Custom mapping strategy is not a mpaping strategy: {0}", MappingStrategyType.AssemblyQualifiedName);
                }
                return result;
            }
        }
    }
}