using System;
using System.Runtime.CompilerServices;
using Xunit;

namespace Memory.Tests
{
    public unsafe class IntPtrUnsafeExtensionsTests
    {

        [Fact]
        public void ToObject_MethodTablePointer_on_string()
        {
            var str = "this is string";
            fixed (char* charsPtr = str)
            {
                // ptr points to char array in str
                var ptr = new IntPtr(charsPtr);
                ptr = IntPtr.Add(ptr, - RuntimeHelpers.OffsetToStringData /*sizeof(int) - IntPtr.Size*/);
                var castedString = ptr.ToObject<string>(ObjectPointerTo.MethodTablePointer);

                Assert.Same(str, castedString);
            }
        }
        [Fact]
        public void ToObject_Header_on_string()
        {
            var str = "this is string";
            fixed (char* charsPtr = str)
            {
                // ptr points to char array in str
                var ptr = new IntPtr(charsPtr);
                ptr = IntPtr.Add(ptr, -sizeof(int) - (IntPtr.Size * 2));
                var castedString = ptr.ToObject<string>(ObjectPointerTo.Header);

                Assert.Same(str, castedString);
            }
        }
        [Fact]
        public void ToObject_Data_on_string()
        {
            var str = "this is string";
            fixed (char* charsPtr = str)
            {
                // ptr points to char array in str
                var ptr = new IntPtr(charsPtr);
                ptr = IntPtr.Add(ptr, -sizeof(int));
                var castedString = ptr.ToObject<string>(ObjectPointerTo.Data);

                Assert.Same(str, castedString);
            }
        }

    }
}
