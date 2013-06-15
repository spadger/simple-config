namespace SimpleConfig.Tests.TestTypes
{
    public class Public_NoObviousConstructor
    { }

    public class Public_PrivateNoArgConstructor
    {
        private Public_PrivateNoArgConstructor() { }
    }

    public class Public_ProtectedNoArgConstructor
    {
        protected Public_ProtectedNoArgConstructor() { }
    }

    public class Public_InternalNoArgConstructor
    {
        internal Public_InternalNoArgConstructor() { }
    }

    public class Public_PublicConstructorWithArgs
    {
        public Public_PublicConstructorWithArgs(int a) { }
    }

    public class Public_PrivateConstructorWithArgs
    {
        private Public_PrivateConstructorWithArgs(int a) { }
    }

    public class Public_ProtectedConstructorWithArgs
    {
        protected Public_ProtectedConstructorWithArgs(int a) { }
    }

    public class Public_InternalConstructorWithArgs
    {
        internal Public_InternalConstructorWithArgs(int a) { }
    }
}