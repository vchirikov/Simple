namespace Memory
{
    /// <summary>
    /// Object address type.
    /// CLR object layout:
    ///                | Default
    /// [objectHeader] V [methodTablePtr] [objectData]
    /// </summary>
    /// <seealso href="https://blogs.msdn.microsoft.com/abhinaba/2012/02/02/wp7-clr-managed-object-overhead/"/>
    /// <seealso href="https://blogs.msdn.microsoft.com/seteplia/2017/09/06/managed-object-internals-part-2-object-header-layout-and-the-cost-of-locking/"/>
    public enum ObjectPointerTo: byte
    {
        /// <summary>
        ///                | POINTER
        /// [objectHeader] V [methodTablePtr] [objectData]
        /// </summary>
        MethodTablePointer = 0,
        /// <summary>
        /// | POINTER
        /// V [objectHeader] [methodTablePtr] [objectData]
        /// </summary>
        Header = 1,
        /// <summary>
        ///                                 | POINTER
        /// [objectHeader] [methodTablePtr] V [objectData]
        /// </summary>
        Data = 2,
    }
}