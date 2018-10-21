using System;

namespace Memory
{
    public static unsafe class ObjectUnsafeExtensions
    {
        /// <summary>
        /// Get <see cref="IntPtr"/> address from <typeparamref name="T"/> <paramref name="obj"/>
        /// with type <paramref name="pointerType"/>
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="obj">source object</param>
        /// <param name="pointerType">type of address</param>
        /// <returns><see cref="IntPtr"/> address with requested type</returns>
        public static IntPtr GetAddress<T>(this T obj, ObjectPointerTo pointerType) where T : class
        {
            if (obj == null)
                return IntPtr.Zero;

            var finder = new FindAddressHelper<T> { Object = obj };
            var addr = &finder.Address;
            IntPtr ptr = *(IntPtr*)&addr[-1];
            switch (pointerType)
            {
                case ObjectPointerTo.MethodTablePointer:
                    break;
                case ObjectPointerTo.Header:
                    ptr = IntPtr.Add(ptr, -IntPtr.Size);
                    break;
                case ObjectPointerTo.Data:
                    ptr = IntPtr.Add(ptr, IntPtr.Size);
                    break;
                default:
                    throw new ArgumentException("Unknown type of requested address", nameof(pointerType));
            }
            return ptr;
        }
    }
}