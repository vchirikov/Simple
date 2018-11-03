using System;
using System.Buffers;
using System.Runtime.InteropServices;

namespace Memory
{
    /// <summary>
    /// A <see cref="MemoryManager{T}"/> over a raw pointer  <para />
    /// The pointer must point to unmanaged memory, or externally pinned
    /// </summary>
    public sealed unsafe class UnmanagedPointerMemoryManager<T> : MemoryManager<T> where T : unmanaged
    {
        private readonly T* _pointer;
        private readonly int _length;

        /// <summary>
        /// Create a new <see cref="UnmanagedPointerMemoryManager{T}"/> instance from <see cref="Span{T}"/>
        /// <paramref name="span"/> must point to unmanaged memory or be externally pinned
        /// </summary>
        public UnmanagedPointerMemoryManager(Span<T> span)
        {
            fixed (T* ptr = &MemoryMarshal.GetReference(span))
            {
                _pointer = ptr;
                _length = span.Length;
            }
        }
        /// <summary>
        /// Create a new <see cref="UnmanagedPointerMemoryManager{T}"/> instance from given pointer and size
        /// </summary>
        /// <param name="pointer">must point to unmanaged memory or be externally pinned</param>
        /// <param name="length">length of data</param>
        public UnmanagedPointerMemoryManager(T* pointer, int length)
        {
            if (length <= 0)
                throw new ArgumentOutOfRangeException(nameof(length));

            _pointer = pointer;
            _length = length;
        }
        /// <summary>
        /// Obtains a <see cref="Span{T}"/> that represents the region
        /// </summary>
        public override Span<T> GetSpan() => new Span<T>(_pointer, _length);

        /// <summary>
        /// Provides access to a pointer that represents the data <para />
        /// No actual pin occurs
        /// </summary>
        public override MemoryHandle Pin(int elementIndex = 0)
        {
            if (elementIndex < 0 || elementIndex >= _length)
                throw new ArgumentOutOfRangeException(nameof(elementIndex));
            return new MemoryHandle(_pointer + elementIndex);
        }

        /// <summary>
        /// Has no effect
        /// </summary>
        public override void Unpin() { }

        /// <summary>
        /// Releases all resources associated with this object
        /// </summary>
        protected override void Dispose(bool disposing) { }
    }
}
