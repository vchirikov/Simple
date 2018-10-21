using System.Runtime.InteropServices;

namespace Memory
{
    /// <summary>
    /// This structure is used for search address of <see cref="FindAddressHelper{T}.Object"/> 
    /// <see cref="LayoutKind.Explicit"/> can't be used, because we use generic<T>
    /// </summary>
    /// <typeparam name="T">Type of object to search</typeparam>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe ref struct FindAddressHelper<T>
    {
        public T Object;
        public fixed byte Address[1];
    }
}