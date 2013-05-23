using System.Reflection;
using System.Xml;

namespace SimpleConfig.BindingStrategies
{
    /// <summary>
    /// Details a specific strategy for populating an object based on the config
    /// </summary>
    public interface IBindingStrategy
    {
        /// <param name="destinationObject">The instanc eof the object ot be populated</param>
        /// <param name="destinationProperty">The property to be populated</param>
        /// <param name="element">The config element at the level we are mapping</param>
        /// <param name="allConfig">The entire config dom, as provided to the config handler</param>
        /// <param name="mapper">The current config mapper</param>
        /// <returns>Whether or not the binding was successful</returns>
        bool Map(object destinationObject, PropertyInfo destinationProperty, XmlElement element, XmlElement allConfig, ConfigMapper mapper);
    }
}