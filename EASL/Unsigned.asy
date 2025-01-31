// Implementation for unsigned numbers.
impl unsigned {

    // Constructors.
    pub inline This() { this = (This)(object)0; }
    pub inline This(This v) { this = v; }
    //pub inline This(signed s) { this = (This)s; }
    //pub inline This(floating f) { this = (This)f; }

    // Classic math operators.
    inline operator +(This a, This b) -> This => (This)llvm("add", a, b);
    inline operator -(This a, This b) -> This => (This)llvm("sub", a, b);
    inline operator *(This a, This b) -> This => (This)llvm("mul", a, b);
    inline operator /(This a, This b) -> This => (This)llvm("udiv", a, b);
    inline operator %(This a, This b) -> This => (This)llvm("urem", a, b);
    inline operator &(This a, This b) -> This => (This)llvm("and", a, b);
    inline operator |(This a, This b) -> This => (This)llvm("or", a, b);
    inline operator ^(This a, This b) -> This => (This)llvm("xor", a, b);
    inline operator ~(This v) -> This => (This)llvm("not", v);
    //inline operator +(This v) -> This => this;
    /*inline operator **(This a, This b) -> This {
        This v = (This)(object)1;
        for (This i = (This)(object)0; i < b; i++)
            v *= a;
        v
    }*/

    // Comparison operators. Even though NE, LE, and GE are not needed, it's more efficient to have them.
    inline operator ==(This a, This b) -> bool => (bool)llvm("icmp", "eq", a, b);
    inline operator !=(This a, This b) -> bool => (bool)llvm("icmp", "ne", a, b);
    inline operator <(This a, This b) -> bool => (bool)llvm("icmp", "ult", a, b);
    inline operator >(This a, This b) -> bool => (bool)llvm("icmp", "ugt", a, b);
    inline operator <=(This a, This b) -> bool => (bool)llvm("icmp", "ule", a, b);
    inline operator >=(This a, This b) -> bool => (bool)llvm("icmp", "uge", a, b);
    inline operator <=>(This a, This b) -> s32 => (s32)llvm("sub", a, b);

    // Conversion functions.
    //inline explicitcast(signed s) -> This => (This)(object)s;
    //inline explicitcast(floating f) -> This => (This)llvm("fptoui", typeof(floating), typeof(This), f);

}