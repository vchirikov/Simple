using System;
using System.Runtime.InteropServices;
using Xunit;

namespace Memory.Tests
{
    public unsafe class UnmanagedMemoryManagerTests
    {
        [Fact]
        public void Ctor_throw_ArgumentOutOfRangeException()
            => Assert.Throws<ArgumentOutOfRangeException>(() => new UnmanagedMemoryManager<byte>(-1));

        [Fact]
        public void Pin()
        {
            using (var mgr = new UnmanagedMemoryManager<char>(3))
            {
                var expected = mgr.Memory.Span;
                expected[0] = 'a';
                expected[1] = 'b';
                expected[2] = 'c';
                using (var second = mgr.Pin(0))
                {
                    var actual00 = *(char*)second.Pointer;
                    var actual01 = *((char*)second.Pointer+1);
                    var actual02 = *((char*)second.Pointer+2);

                    Assert.Equal(expected[0], actual00);
                    Assert.Equal(expected[1], actual01);
                    Assert.Equal(expected[2], actual02);
                }
            }

        }
        [Fact]
        public void Disposed_throw_ObjectDisposedException()
        {
            var mgr = new UnmanagedMemoryManager<char>(1);
            ((IDisposable)mgr).Dispose();
            Assert.Throws<ObjectDisposedException>(() => mgr.Pin());
            Assert.Throws<ObjectDisposedException>(() => mgr.Unpin());
            Assert.Throws<ObjectDisposedException>(() => mgr.GetSpan());

        }

        [Fact]
        public void Pin_throw_ArgumentOutOfRangeException()
        {
            using (var mgr = new UnmanagedMemoryManager<byte>(1))
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => mgr.Pin(-1));
            }
        }
    }
}
