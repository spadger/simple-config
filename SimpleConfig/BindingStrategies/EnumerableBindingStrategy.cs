using System.Xml;

namespace SimpleConfig.BindingStrategies
{
    public class EnumerableBindingStrategy : IBindingStrategy
    {
        public bool Do<T>(T destination, XmlElement element, XmlElement allConfig)
        {
            return true;
        }
    }
}