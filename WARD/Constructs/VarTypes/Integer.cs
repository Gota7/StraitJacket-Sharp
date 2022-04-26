using System;
using LLVMSharp.Interop;

namespace WARD.Constructs {

    // A signed or unsigned integer.
    public class VarTypeInteger : VarType {
        public bool Signed;
        public uint BitWidth;
        
        public VarTypeInteger(bool signed, uint bitWidth) {
            Type = VarTypeEnum.PrimitiveInteger;
            Signed = signed;
            BitWidth = bitWidth;
        }

        protected override LLVMTypeRef LLVMType() {
            return LLVMTypeRef.CreateInt(BitWidth);
        }

        protected override string Mangled() => (Signed ? "i" : "u") + BitWidth.ToString() + "E";

        public override bool CanImplicitlyCastTo(VarType other) {
            var otherInt = other as VarTypeInteger;
            var otherFixed = other as VarTypeFixed;
            if (otherInt != null) {
                return otherInt.BitWidth > BitWidth || otherInt.BitWidth == BitWidth && otherInt.Signed == Signed;
            } else if (other.Type == VarTypeEnum.PrimitiveFloating) {
                return true;
            } else if (other.Type == VarTypeEnum.PrimitiveFixed) {
                return otherFixed.WholeWidth > BitWidth || otherFixed.WholeWidth == BitWidth && Signed;
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

        public override LLVMValueRef CastTo(LLVMValueRef srcVal, VarType destType, LLVMModuleRef mod, LLVMBuilderRef builder) {
            if (destType.Type == VarTypeEnum.PrimitiveInteger) {
                var src = this;
                var dest = destType as VarTypeInteger;
                if (src.BitWidth < dest.BitWidth) {
                    if (src.Signed) {
                        return builder.BuildSExt(srcVal, destType.GetLLVMType(), "SJ_CastInt_SExt");
                    } else {
                        return builder.BuildZExt(srcVal, destType.GetLLVMType(), "SJ_CastInt_ZExt");
                    }
                } else if (src.BitWidth > dest.BitWidth) {
                    return builder.BuildTrunc(srcVal, dest.GetLLVMType(), "SJ_CastInt_Trunc");
                } else {
                    return srcVal;
                }
            } else if (destType.Type == VarTypeEnum.PrimitiveFloating) {
                var src = this;
                var dest = destType as VarTypeFloating;
                if (src.Signed) {
                    return builder.BuildSIToFP(srcVal, destType.GetLLVMType(), "SJ_CastInt_Float");
                } else {
                    return builder.BuildUIToFP(srcVal, destType.GetLLVMType(), "SJ_CastUInt_Float");
                }
            } else if (destType.Type == VarTypeEnum.PrimitiveFixed) {
                var src = this;
                var dest = destType as VarTypeFixed;
                LLVMValueRef tmp = srcVal;
                if (dest.WholeWidth + dest.FractionWidth > BitWidth) {
                    if (src.Signed) {
                        tmp = builder.BuildSExt(tmp, dest.GetLLVMType());
                    } else {
                        tmp = builder.BuildZExt(tmp, dest.GetLLVMType());
                    }
                } else if (dest.WholeWidth + dest.FractionWidth < BitWidth) {
                    tmp = builder.BuildTrunc(tmp, dest.GetLLVMType());
                }
                return builder.BuildShl(tmp,
                    LLVMValueRef.CreateConstInt(tmp.TypeOf, dest.FractionWidth, false), "SJ_CastInt_Fixed"
                );
            }
            return base.CastTo(srcVal, destType, mod, builder);
        }

        public override bool Equals(object obj) {
            if (obj is VarTypeCustom) return Equals((obj as VarTypeCustom).Resolved);
            if (obj is VarTypeInteger) {
                var i = obj as VarTypeInteger;
                if (i.Constant != Constant) return false;
                if (i.Atomic != Atomic) return false;
                if (i.Volatile != Volatile) return false;
                return i.BitWidth == BitWidth && i.Signed == Signed;
            }
            return false;
        }

        public override int GetHashCode() {
            HashCode hash = new HashCode();
            hash.Add(Type);
            hash.Add(Constant);
            hash.Add(Volatile);
            hash.Add(Atomic);
            hash.Add(BitWidth);
            hash.Add(Signed);
            return hash.ToHashCode();
        }

        public override string ToString() {
            return base.ToString() + (Signed ? "s" : "u") + BitWidth;
        }

        public override Expression DefaultValue() {
            return new ExpressionConstInt(Signed, 0);
        }

    }

}