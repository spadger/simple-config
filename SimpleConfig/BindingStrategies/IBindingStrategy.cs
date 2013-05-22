using System.Xml;

namespace SimpleConfig.BindingStrategies
{
    /// <summary>
    /// Details a specific strategy for populating an object based on the config
    /// </summary>
    public interface IBindingStrategy
    {
        /// <typeparam name="T">The type of object to be mapped</typeparam>
        /// <param name="destination">The object to be mapped</param>
        /// <param name="element">The config element at the level we are mapping</param>
        /// <param name="allConfig">The entire config dom, as provided to the config handler</param>
        /// <returns>Whether or not the binding was successful</returns>
        bool Do<T>(T destination, XmlElement element, XmlElement allConfig);
    }
}