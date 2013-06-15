namespace SimpleConfig.Tests.TestTypes
{
    internal class Internal_NoObviousConstructor
    { }

    internal class Internal_PrivateNoArgConstructor
    {
        private Internal_PrivateNoArgConstructor() { }
    }

    internal class Internal_ProtectedNoArgConstructor
    {
        protected Internal_ProtectedNoArgConstructor() { }
    }

    internal class Internal_InternalNoArgConstructor
    {
        internal Internal_InternalNoArgConstructor() { }
    }

    internal class Internal_PublicConstructorWithArgs
    {
        public Internal_PublicConstructorWithArgs(int a) { }
    }

    internal class Internal_PrivateConstructorWithArgs
    {
        private Internal_PrivateConstructorWithArgs(int a) { }
    }

    internal class Internal_ProtectedConstructorWithArgs
    {
        protected Internal_ProtectedConstructorWithArgs(int a) { }
    }

    internal class Internal_InternalConstructorWithArgs
    {
        internal Internal_InternalConstructorWithArgs(int a) { }
    } 
}