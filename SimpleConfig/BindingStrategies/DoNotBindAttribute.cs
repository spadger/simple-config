using System;

namespace SimpleConfig.BindingStrategies
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DoNotBindAttribute : BaseBindingAttribute
    {
        public override IBindingStrategy MappingStrategy
        {
            get { throw new NotImplementedException(); }
        }
    }
}