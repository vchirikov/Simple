using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Xunit;

namespace Memory.Tests
{
    public unsafe class UnmanagedPointerMemoryManagerTests
    {
        [Fact]
        public void Ctor_from_pointer()
        {
            string str = "abc";
            fixed (char* charsPtr = str)
            {
                var mgr = new UnmanagedPointerMemoryManager<char>(charsPtr,  str.Length);
                Assert.Equal('a', mgr.Memory.Span[0]);
            }
        }

        [Fact]
        public void GetSpan_cctor_from_span()
        {
            var array = new byte[] { 31, 33, 7};
            var span = new Span<byte>(array);
            fixed (byte* ptr = array)
            {
                var mgr = new UnmanagedPointerMemoryManager<byte>(span);
                var mgrSpan = mgr.GetSpan();
                Assert.Equal( span[0], mgrSpan[0]);
                Assert.Equal(array[1], mgrSpan[1]);
                Assert.Equal(array[2], mgrSpan[2]);
            }
        }

        [Fact]
        public void Pin()
        {
            var array = new byte[] { 31, 33, 7};
            var span = new Span<byte>(array);
            fixed (byte* ptr = array)
            {
                var mgr = new UnmanagedPointerMemoryManager<byte>(span);
                using (var handle00 = mgr.Pin())
                using (var handle01 = mgr.Pin(1))
                {
                    Assert.Equal((ulong) ptr, (ulong)handle00.Pointer);
                    Assert.Equal((ulong) (ptr+1), (ulong)handle01.Pointer);
                }
            }
        }

        [Fact]
        public void Pin_throw_ArgumentOutOfRangeException()
        {
            var array = new byte[] { 31, 33, 7};
            var span = new Span<byte>(array);
            fixed (byte* ptr = array)
            {
                var mgr = new UnmanagedPointerMemoryManager<byte>(span);
                Assert.Throws<ArgumentOutOfRangeException>(() => mgr.Pin(-1));
            }
        }
    }
}
