using System;
using System.Buffers;
using System.Runtime.InteropServices;

namespace Memory
{
    public sealed unsafe class UnmanagedMemoryManager<T> : MemoryManager<T> where T : unmanaged
    {
        private readonly IntPtr _memoryPointer;
        private UnmanagedPointerMemoryManager<T> _pointerManager;
        private readonly int _length;
        private readonly int _byteLength;

        private bool isDisposed = false;

        public UnmanagedMemoryManager(int length)
        {
            if (length <= 0)
                throw new ArgumentOutOfRangeException();

            _length = length;
            _byteLength = SizeOf(typeof(T)) * length;
            _memoryPointer = Marshal.AllocHGlobal(_byteLength);
            _pointerManager = new UnmanagedPointerMemoryManager<T>((T*)_memoryPointer.ToPointer(), length);
            GC.AddMemoryPressure(_byteLength);
        }

        private static int SizeOf(Type type)
        {
            if (type == typeof(byte))
                return sizeof(byte);

            if (type == typeof(char))
                return sizeof(char);

            if (type == typeof(int) || type == typeof(uint))
                return sizeof(int);

            if (type == typeof(long) || type == typeof(ulong))
                return sizeof(long);

            if (type == typeof(IntPtr) || type == typeof(void*))
                return IntPtr.Size;

            if (type == typeof(short) || type == typeof(ushort))
                return sizeof(short);

            return Marshal.SizeOf(type);

        }

        /// <summary>
        /// Obtains a <see cref="Span{T}"/> that represents the region
        /// </summary>
        public override Span<T> GetSpan()
        {
            if (isDisposed)
                throw new ObjectDisposedException(nameof(UnmanagedMemoryManager<T>));
            return _pointerManager.GetSpan();
        }

        /// <summary>
        /// Provides access to a pointer that represents the data <para />
        /// No actual pin occurs
        /// </summary>
        public override MemoryHandle Pin(int elementIndex = 0)
        {
            if (isDisposed)
                throw new ObjectDisposedException(nameof(UnmanagedMemoryManager<T>));
            return _pointerManager.Pin(elementIndex);
        }

        /// <summary>
        /// Has no effect
        /// </summary>
        public override void Unpin()
        {
            if (isDisposed)
                throw new ObjectDisposedException(nameof(UnmanagedMemoryManager<T>));
        }

        /// <summary>
        /// Releases all resources associated with this object
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                isDisposed = true;
                Marshal.FreeHGlobal(_memoryPointer);
                GC.RemoveMemoryPressure(_byteLength);
                _pointerManager = null;

            }

        }
    }

}
