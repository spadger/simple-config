using System.Reflection;
using System.Xml;

namespace SimpleConfig.BindingStrategies
{
    public class ComplexTypeBindingStrategy : IBindingStrategy
    {
        public bool Map(object destinationObject, PropertyInfo destinationProperty, XmlElement element, XmlElement allConfig, ConfigMapper mapper)
        {
            return true;
        }
    }
}