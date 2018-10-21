using System;

namespace Memory
{
    public static unsafe class IntPtrUnsafeExtensions
    {
        public static Span<T> AsSpan<T>(this IntPtr ptr, int length) => new Span<T>(ptr.ToPointer(), length);
        public static Span<T> AsSpan<T>(this IntPtr ptr, int offset, int length)
            => new Span<T>(IntPtr.Add(ptr, offset).ToPointer(), length);

        /// <summary>
        /// Cast <see cref="IntPtr"/> to <typeparamref name="TResult"/>
        /// </summary>
        /// <typeparam name="TResult">result type</typeparam>
        /// <param name="address">pointer to object</param>
        /// <param name="pointerType">address type</param>
        /// <returns>casted object</returns>
        public static TResult ToObject<TResult>(this IntPtr address, ObjectPointerTo pointerType) where TResult : class
        {
            var finder = new FindAddressHelper<TResult>();
            var addr = &finder.Address;
            var ptr = address;
            if(ptr != IntPtr.Zero)
            {
                switch (pointerType)
                {
                    case ObjectPointerTo.MethodTablePointer:
                        break;
                    case ObjectPointerTo.Header:
                        ptr = IntPtr.Add(ptr, IntPtr.Size);
                        break;
                    case ObjectPointerTo.Data:
                        ptr = IntPtr.Add(ptr, -IntPtr.Size);
                        break;
                    default:
                        throw new ArgumentException("Unknown type of address", nameof(pointerType));
                }
            }
            *(IntPtr*)&addr[-1] = ptr;
            return finder.Object;
        }
    }
}