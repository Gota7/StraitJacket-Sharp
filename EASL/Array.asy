pub unsafe struct Array<T> {
    pub T* members { get; private set; }
    pub size_t length { get; private set; } // Maybe make it a pointer to lengths for multi-dimensional support?
}