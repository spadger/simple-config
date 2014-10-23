namespace SimpleConfig.Tests.TestTypes
{
    public interface SomeInterfaces { }

    public interface WithMethod { void X(); }
    public interface NoGetter { int X { set; } }
    public interface NoSetter { int X { get; } }
    public interface NoProperties { }
}
