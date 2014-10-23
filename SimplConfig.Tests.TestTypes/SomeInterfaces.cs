namespace SimpleConfig.Tests.TestTypes
{
    public interface SomeInterfaces { }

    public interface PropertyWithMethod { void X(); }
    public interface NoPublicGetter { int X { set; } }
    public interface NoPublicSetter { int X { get; } }
    public interface NoProperties { }
}
