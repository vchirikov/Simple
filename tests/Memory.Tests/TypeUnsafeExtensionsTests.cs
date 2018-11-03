using System;
using Xunit;

namespace Memory.Tests
{
    public unsafe class TypeUnsafeExtensionsTests
    {
        [Fact]
        public void GetTypeSize_on_class()
        {
            var fooSize = typeof(Foo).GetTypeSize();

            Assert.Equal(fooSize, /* object header */ IntPtr.Size
                                + /* reference to method table */ IntPtr.Size
                                + /* reference to 3 strings */ (IntPtr.Size * 3)
                                + /* 1 char */ 1
                                + /* 7 byte (x64) or 3 byte (x86) padding */ IntPtr.Size - 1);
        }

        [Fact]
        public void GetTypeSize_on_int()
        {
            // must return sizeof(boxed int)
            var fooSize = typeof(int).GetTypeSize();

            Assert.Equal(fooSize, /* object header */ IntPtr.Size
                                + /* reference to method table */ IntPtr.Size
                                + /* int size */ sizeof(int)
                                + /* padding */ IntPtr.Size - sizeof(int));
        }

        private class Foo
        {
            public string Test1 { get; set; }
            public string Test2 { get; set; }
            public string Test3 { get; set; }
            public char ContainsA = 'A';
            public char ContainsA1 = 'A';
            public char ContainsA2 = 'A';
        }

    }
}
