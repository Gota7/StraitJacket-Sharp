using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using LLVMSharp;
using LLVMSharp.Interop;

namespace StraitJacketLib.Constructs {

    // Number type.
    public enum NumberType {
        Whole,
        Decimal
    }

    // A number.
    public class Number {
        public bool ForceSigned;
        public NumberType Type;
        public long ValueWhole;
        public double ValueDecimal;
        public uint MinBits => ForceSigned ? ((uint)Math.Log2(Math.Abs(ValueWhole)) + 2) : ((uint)Math.Log2((ulong)ValueWhole) + 1);
    }

    // Primitives.
    public enum Primitives {
        String,
        Bool,
        Unsigned,
        Signed,
        Half,
        Float,
        Double,
        Extended,
        Decimal,
        Fixed,
        VariableLength,
        Object,
        Void,
        Function,
        Event,
        Char,
        WideChar,
        UnsignedAny,
        SignedAny,
        FloatingAny,
        FixedAny
    }

    // Variable type enum
    public enum VarTypeEnum {
        PrimitiveSimple,
        PrimitiveInteger,
        PrimitiveFixed,
        PrimitiveFunction,
        Tuple,
        Pointer,
        Reference,
        Array,
        Custom,
        Generics
    }

    // Variable type.
    public abstract class VarType : IEqualityComparer<VarType> {
        public VarTypeEnum Type;
        public bool Constant;
        public bool Static;
        public bool Volatile;
        public bool Atomic;
        public bool Variadic;
        private bool TypeNotGotten = true;
        private LLVMTypeRef GottenType = null;

        // Disallow external creation.
        protected VarType() {}

        // For each member to get the LLVM type.
        protected abstract LLVMTypeRef LLVMType();

        // Get the mangled version.
        protected abstract string Mangled();

        // Get the LLVM type.
        public LLVMTypeRef GetLLVMType() {
            if (TypeNotGotten) {
                GottenType = LLVMType();
            }
            if (GottenType == null) throw new Exception("BAD TYPE!!!");
            return GottenType;
        }

        // If the type is floating point.
        public bool IsFloatingPoint() {
            var val = this as VarTypeSimplePrimitive;
            if (val != null) {
                SimplePrimitives prim = val.Primitive;
                return prim == SimplePrimitives.Half || prim == SimplePrimitives.Float || prim == SimplePrimitives.Double || prim == SimplePrimitives.Extended || prim == SimplePrimitives.Decimal;
            }
            return false;
        }

        // If the type is fixed.
        public bool IsFixed() {
            return this as VarTypeFixed != null;
        }

        // If the type is unsigned.
        public bool IsUnsigned() {
            var val = this as VarTypeInteger;
            if (val != null) {
                return !val.Signed;
            }
            return false;
        }

        // If the type is signed.
        public bool IsSigned() {
            var val = this as VarTypeInteger;
            if (val != null) {
                return val.Signed;
            }
            return false;
        }

        // If type can be implicitly casted to another. TODO: CUSTOM CONVERSIONS!!!
        public virtual bool CanImplicitlyCastTo(VarType other) {
            if (other.Equals(this)) return true;
            if (other.Type == VarTypeEnum.Custom) return CanImplicitlyCastTo((other as VarTypeCustom).Resolved);
            return other.Equals(new VarTypeSimplePrimitive(SimplePrimitives.Object)) || Equals(new VarTypeSimplePrimitive(SimplePrimitives.Object));
        }

        // If type can be casted to another.
        public virtual bool CanCastTo(VarType other) {
            if (other.Equals(this)) return true;
            if (other.Type == VarTypeEnum.Custom) return CanCastTo((other as VarTypeCustom).Resolved);
            return other.Equals(new VarTypeSimplePrimitive(SimplePrimitives.Object)) || Equals(new VarTypeSimplePrimitive(SimplePrimitives.Object));
        }

        // Cast to another type. TODO!!!
        public virtual ReturnValue CastTo(ReturnValue srcVal, VarType destType, LLVMModuleRef mod, LLVMBuilderRef builder) {
            if (destType.Equals(this)) return srcVal;
            if (destType.Type == VarTypeEnum.Custom) return CastTo(srcVal, (destType as VarTypeCustom).Resolved, mod, builder);
            if (destType.Equals(new VarTypeSimplePrimitive(SimplePrimitives.Object)) || Equals(new VarTypeSimplePrimitive(SimplePrimitives.Object))) {
                return srcVal;
            } else {
                return null;
            }
        }

        // Get the mangled type.
        public string GetMangled() {
            string ret = "";
            if (Constant) ret += "C";
            if (Static) ret += "S";
            if (Volatile) ret += "V";
            if (Atomic) ret += "A";
            if (Variadic) ret += "I";
            return ret + Mangled();
        }

        public bool Equals(VarType x, VarType y) {
            if (x.Type != y.Type) return false;
            if (x.Type == VarTypeEnum.PrimitiveSimple) return ((VarTypeSimplePrimitive)x).Equals(y);
            else if (x.Type == VarTypeEnum.PrimitiveInteger) return ((VarTypeInteger)x).Equals(y);
            else if (x.Type == VarTypeEnum.PrimitiveFixed) return ((VarTypeFixed)x).Equals(y);
            else if (x.Type == VarTypeEnum.PrimitiveFunction) return ((VarTypeFunction)x).Equals(y);
            else if (x.Type == VarTypeEnum.Tuple) return ((VarTypeTuple)x).Equals(y);
            return false;
        }

        public int GetHashCode([DisallowNull] VarType x) {
            if (x.Type == VarTypeEnum.PrimitiveSimple) return ((VarTypeSimplePrimitive)x).GetHashCode();
            else if (x.Type == VarTypeEnum.PrimitiveInteger) return ((VarTypeInteger)x).GetHashCode();
            else if (x.Type == VarTypeEnum.PrimitiveFixed) return ((VarTypeFixed)x).GetHashCode();
            else if (x.Type == VarTypeEnum.PrimitiveFunction) return ((VarTypeFunction)x).GetHashCode();
            else if (x.Type == VarTypeEnum.Tuple) return ((VarTypeTuple)x).GetHashCode();
            return 0;
        }

        public override string ToString() {
            string ret = "";
            if (Constant) ret += "const ";
            if (Static) ret += "static ";
            if (Volatile) ret += "volatile ";
            if (Atomic) ret += "atomic ";
            if (Variadic) ret += "variadic ";
            return ret;
        }

    }

    // Variable parameter.
    public class VarParameter {
        public string Label;
        public Variable Value = new Variable();
        public List<LLVMValueRef> VariadicArgs;
    }

}