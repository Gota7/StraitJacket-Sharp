using System;
using System.Collections.Generic;
using LLVMSharp.Interop;

namespace StraitJacketLib.Constructs {

    // A floating point value.
    public class VarTypeFloating : VarType {
        public uint BitWidth;
        public readonly List<uint> SupportedWidths = new List<uint> { 16, 32, 64, 80, 128 };
        public readonly Dictionary<uint, LLVMTypeRef> LLVMTypes = new Dictionary<uint, LLVMTypeRef>() {
            { 16, LLVMTypeRef.Half },
            { 32, LLVMTypeRef.Float },
            { 64, LLVMTypeRef.Double },
            { 80, LLVMTypeRef.X86FP80 },
            { 128, LLVMTypeRef.FP128 }
        };
        public readonly Dictionary<uint, string> MangledWidths = new Dictionary<uint, string>() {
            { 16, "h" },
            { 32, "f" },
            { 64, "d" },
            { 80, "e" },
            { 128, "c" }
        };

        public VarTypeFloating(uint bitWidth) {
            Type = VarTypeEnum.PrimitiveFloating;
            if (!SupportedWidths.Contains(bitWidth)) throw new Exception("Unsupported floating point bit-width!");
            BitWidth = bitWidth;
        }

        protected override LLVMTypeRef LLVMType() => LLVMTypes[BitWidth];

        protected override string Mangled() => MangledWidths[BitWidth];

        public override bool CanImplicitlyCastTo(VarType other) {
            var fp = other as VarTypeFloating;
            if (fp != null) {
                return fp.BitWidth > BitWidth;
            } else {
                return base.CanImplicitlyCastTo(other);
            }
        }

        public override bool CanCastTo(VarType other) {
            if (other.IsFixed() || other.IsFloatingPoint() || other.IsUnsigned() || other.IsSigned()) {
                return true;
            } else {
                return base.CanCastTo(other);
            }
        }

        public override ReturnValue CastTo(ReturnValue srcVal, VarType destType, LLVMModuleRef mod, LLVMBuilderRef builder) {
            if (destType.Type == VarTypeEnum.PrimitiveFloating) {
                var src = this;
                var dest = destType as VarTypeFloating;
                if (src.BitWidth < dest.BitWidth) {
                    return new ReturnValue(builder.BuildFPExt(srcVal.Val, dest.GetLLVMType(), "SJ_CastFloat_Ext"));
                } else if (src.BitWidth > dest.BitWidth) {
                    return new ReturnValue(builder.BuildFPTrunc(srcVal.Val, dest.GetLLVMType(), "SJ_CastFloat_Trunc"));
                } else {
                    return srcVal;
                }
            }
            return base.CastTo(srcVal, destType, mod, builder);
        }

        public override bool Equals(object obj) {
            if (obj is VarTypeCustom) return Equals((obj as VarTypeCustom).Resolved);
            if (obj is VarTypeFloating) {
                var i = obj as VarTypeFloating;
                if (i.Constant != Constant) return false;
                if (i.Atomic != Atomic) return false;
                if (i.Volatile != Volatile) return false;
                if (i.Variadic != Variadic) return false;
                return i.BitWidth == BitWidth;
            }
            return false;
        }

        public override int GetHashCode() {
            HashCode hash = new HashCode();
            hash.Add(Type);
            hash.Add(Constant);
            hash.Add(Volatile);
            hash.Add(Atomic);
            hash.Add(Variadic);
            hash.Add(BitWidth);
            return hash.ToHashCode();
        }

        public override string ToString() {
            return base.ToString() + "f" + BitWidth;
        }

        public override Expression DefaultValue() {
            return new ExpressionConstFloat(BitWidth, 0);
        }

    }

}