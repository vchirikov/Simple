using System;
using System.Runtime.CompilerServices;
using Xunit;

namespace Memory.Tests
{
    public unsafe class ObjectUnsafeExtensionsTests
    {
        [Fact]
        public void GetAddress_MethodTablePointer_on_string()
        {
            string str = "abc";
            var methodTablePtrWithDataLen = ((str.Length + 1) * sizeof(char)) + sizeof(int) + IntPtr.Size;
            var actualPtr = str.GetAddress(ObjectPointerTo.MethodTablePointer);

            fixed (char* charsPtr = str)
            {
                var bytesPtr = (byte*)charsPtr;
                // bytesPtr/charsPtr points to {'a','a','a'}, without len field and methodTablePtr
                var expectedPtr = bytesPtr - RuntimeHelpers.OffsetToStringData; // sizeof(int) - IntPtr.Size;

                var expectedPtrSpan = new Span<byte>(expectedPtr, methodTablePtrWithDataLen);
                var actualPtrSpan = actualPtr.AsSpan<byte>(methodTablePtrWithDataLen);

                Assert.Equal((long)expectedPtr, (long)actualPtr);
            }
        }

        [Fact]
        public void GetAddress_Data_on_string()
        {
            string str = "abc";
            var objectDataLen = ((str.Length + 1) * sizeof(char)) + sizeof(int);
            var actualPtr = str.GetAddress(ObjectPointerTo.Data);

            fixed (char* charsPtr = str)
            {
                var bytesPtr = (byte*)charsPtr;
                // bytesPtr/charsPtr points to {'a','a','a'}, without len field and methodTablePtr
                var expectedPtr = bytesPtr - sizeof(int);

                var expectedPtrSpan = new Span<byte>(expectedPtr, objectDataLen);
                var actualPtrSpan = actualPtr.AsSpan<byte>(0, objectDataLen);

                Assert.Equal((long)expectedPtr, (long)actualPtr);
            }
        }

        [Fact]
        public void GetAddress_Header_on_string()
        {
            string str = "abc";
            var objWithHeaderLen = ((str.Length + 1) * sizeof(char)) + sizeof(int) + (IntPtr.Size * 2);
            var actualPtr = str.GetAddress(ObjectPointerTo.Header);

            fixed (char* charsPtr = str)
            {
                var bytesPtr = (byte*)charsPtr;
                // bytesPtr/charsPtr points to {'a','a','a'}, without len field and methodTablePtr, obj header
                var expectedPtr = bytesPtr - sizeof(int) - (IntPtr.Size * 2);

                var expectedPtrSpan = new Span<byte>(expectedPtr, objWithHeaderLen);
                var actualPtrSpan = actualPtr.AsSpan<byte>(objWithHeaderLen);

                Assert.Equal((long)expectedPtr, (long)actualPtr);
            }
        }


    }
}
