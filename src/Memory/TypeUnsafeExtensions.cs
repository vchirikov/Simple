using System;

namespace Memory
{
    public static unsafe class TypeUnsafeExtensions
    {
        /// <summary>
        /// Read base size of instance of this class when allocated on the heap
        /// </summary>
        /// <param name="type">type to read information</param>
        /// <returns>Size of instance in bytes</returns>
        /// <seealso href="https://github.com/dotnet/coreclr/blob/master/src/vm/methodtable.h"/>
        // Read `DWORD m_BaseSize;`
        public static int GetTypeSize(this Type type) => ((int*)type.TypeHandle.Value)[1];
    }
}