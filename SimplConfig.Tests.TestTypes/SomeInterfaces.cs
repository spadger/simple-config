namespace SimpleConfig.Tests.TestTypes
{
    public interface SomeInterfaces { }

    public interface WithMethod { void X(); }
    public interface NoGetter { int X { set; } }
    public interface NoSetter { int X { get; } }
    public interface GetterAndSetter { int X { get; set; } }
    public interface GetterAndSetterAlternate { int X { get; set; } }
    public interface NoProperties { }
    public interface SomeBindableInterface{ int Value { get; set; } }
}