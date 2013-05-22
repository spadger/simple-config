using System;

namespace SimpleConfig.BindingStrategies
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public abstract class BaseBindingAttribute : Attribute
    {
        public abstract IBindingStrategy MappingStrategy { get; }
    }
}