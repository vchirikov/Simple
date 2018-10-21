using System;

namespace Memory
{
    public static unsafe class TypeUnsafeExtensions
    {
        public static int GetTypeSize(this Type type) => ((int*)type.TypeHandle.Value)[1];
    }
}