simple-config
=============

Simple-config is an extensible, convention-based XML to C# binder, specifically designed to easily bind custom config sections without the need to write any config handlers or sections.

Simply by performing a cast to the required type, SimpleConfig will perform all required mapping, without the use for any magical markup

[![Nuget](https://img.shields.io/nuget/dt/simple-config.svg)](https://www.nuget.org/packages/simple-config/)
[![Nuget](https://img.shields.io/nuget/v/simple-config.svg)](https://www.nuget.org/packages/simple-config/)
[![Build status](https://ci.appveyor.com/api/projects/status/9arg9hw0admmlkox/branch/master?svg=true)](https://ci.appveyor.com/project/spadger/simple-config/branch/master)

##Simple use case

If we have some configurable settings
```C#
public class ServiceSettings
{
  public int MaxThreads { get; set; }
  public string Endpoint { get; set; }
  public IEnumerable<string> BannedPhrases { get; set; }
}
```
we could write the xml for it in our app.config or web.config
```xml
<!-- Wire up the handler inside the <configSections /> element; once per custom section -->
<section name="serviceSettings" type="SimpleConfig.SimpleConfigHandler, SimpleConfig" />

<!-- And this goes in the main part of the config file -->
<serviceSettings maxThreads="4">
  <endpoint>http://localhost:9090</endpoint>
  <bannedPhrases>
    <phrase>something</phrase>
    <phrase>else</phrase>
  </bannedPhrases>
</serviceSettings>
```

We could now write a disproportionately large ConfigurationSection, or a some boilerplate code, or we could just call this:

```C#
var settings = (ServiceSettings)(dynamic)ConfigurationManager.GetSection("serviceSettings");
```
or even
```C#
ServiceSettings settings = (dynamic)ConfigurationManager.GetSection("serviceSettings");
```

##Binding to interfaces
As of version 1.2 (Nov 2014), it is possible to bind to a pure interface (a concrete class will be create for you).  The binding copes with subinterfaces and multiple inheritence too; you just need to remember the following points:

    * Your interface cannot define any methods
    * Each property must define a getter (setters are optional)
    * You must use implicit binding to invoke the mapper:

```C#
IServiceSettings settings = (dynamic)ConfigurationManager.GetSection("serviceSettings");
```

##Overriding the default conventions
If you don't mind using attributes, Simple-config does come with some a small selection of binding hints to guide the binding process.  For example, using the same xml as above, the following settings DTO could still be bound:

```C#
public class ServiceSettings
{
  public int MaxThreads { get; set; }
  public string Endpoint { get; set; }
  
  [CustomEnumerable("bannedPhrases")]  //magic
  public IEnumerable<string> PhrasesThatAreBanned { get; set; }
}
```

##Enumerables and lists
Simple-config is designed to be helpful when binding IEnumerables; please consider the following when binding:
  * You destination needs to implement System.Collections.Generic.IEnumerable<> so that the payload type is known
  * You can only bind to IEnumerable<> if the destination property has a setter (so SimpleConfig can create a mutatable Type)
  * If your destination property has no setter, it needs to be pre-populated with something that implements ICollection<>

##Do anything

The architecture of Simple-config allows you to create new binding strategies to perform whatever custom binding you need, for example, to decrypt sensitive config

Simply create a binding stratgy that implements IBindingStrategy

```C#
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
```

In order to hook up the binding strategy, creatw a new binding attribte that inherits BaseBindingAttribute

```C#
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public abstract class BaseBindingAttribute : Attribute
{
    public abstract IBindingStrategy MappingStrategy { get; }
}
```

Then attach the binding attribute to the property that requires custom binding
