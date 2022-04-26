using System;
using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Fixed type integer.
    public class VarTypeFixed : VarType {
        public uint WholeWidth;
        public uint FractionWidth;

        public VarTypeFixed(uint wholeWidth, uint fractionWidth) {
            Type = VarTypeEnum.PrimitiveFixed;
            WholeWidth = wholeWidth;
            FractionWidth = fractionWidth;
        }

        protected override LLVMTypeRef LLVMType() {
            return LLVMTypeRef.CreateInt(WholeWidth + FractionWidth);
        }

        protected override string Mangled() => "x" + WholeWidth.ToString() + "X" + FractionWidth.ToString() + "E";

        public override bool CanImplicitlyCastTo(VarType other) {
            var f = other as VarTypeFixed;
            if (f != null) {
                return f.WholeWidth >= WholeWidth && f.FractionWidth >= FractionWidth;
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
            if (destType.Type == VarTypeEnum.PrimitiveFixed) {
                var src = this;
                var dest = destType as VarTypeFixed;
                LLVMValueRef tmp = srcVal;
                if (dest.WholeWidth + dest.FractionWidth > src.WholeWidth + src.FractionWidth) {
                    tmp = builder.BuildSExt(tmp, dest.GetLLVMType());
                }
                if (dest.FractionWidth > src.FractionWidth) {
                    tmp = builder.BuildShl(tmp, new ExpressionConstInt(false, dest.FractionWidth - src.FractionWidth).Compile(mod, builder, null));
                } else if (dest.FractionWidth < src.FractionWidth) {
                    tmp = builder.BuildLShr(tmp, new ExpressionConstInt(false, src.FractionWidth - dest.FractionWidth).Compile(mod, builder, null));
                    // TODO: ADD .5!!!
                }
                if (dest.WholeWidth + dest.FractionWidth < src.WholeWidth + src.FractionWidth) {
                    tmp = builder.BuildTrunc(tmp, dest.GetLLVMType());
                }
                return tmp;
            } else if (destType.Type == VarTypeEnum.PrimitiveFloating) {
                var src = this;
                var dest = destType as VarTypeFloating;
                Expression tmpVal = new ExpressionConstInt(false, (long)(1 << (int)src.FractionWidth));
                LLVMValueRef tmp = tmpVal.Compile(mod, builder, null);
                tmp = builder.BuildUIToFP(tmp, dest.GetLLVMType());
                LLVMValueRef tmp2 = builder.BuildSIToFP(srcVal, dest.GetLLVMType());
                return builder.BuildFDiv(tmp2, tmp, "SJ_CastFixed_Float");
            } else if (destType.Type == VarTypeEnum.PrimitiveInteger) {
                var src = this;
                var dest = destType as VarTypeInteger;
                LLVMValueRef tmp = builder.BuildLShr(srcVal,
                    LLVMValueRef.CreateConstInt(srcVal.TypeOf, src.FractionWidth, false)
                );
                if (src.WholeWidth + src.FractionWidth > dest.BitWidth) {
                    return builder.BuildTrunc(tmp, dest.GetLLVMType(), "SJ_CastFixed_Int");
                } else if (src.WholeWidth + src.FractionWidth < dest.BitWidth) {
                    return builder.BuildSExt(tmp, dest.GetLLVMType(), "SJ_CastFixed_Int");
                } else {
                    return tmp;
                }
            }
            return base.CastTo(srcVal, destType, mod, builder);
        }

        public override bool Equals(object obj) {
            if (obj is VarTypeCustom) return Equals((obj as VarTypeCustom).Resolved);
            if (obj is VarTypeFixed) {
                var i = obj as VarTypeFixed;
                if (i.Constant != Constant) return false;
                if (i.Atomic != Atomic) return false;
                if (i.Volatile != Volatile) return false;
                return i.WholeWidth == WholeWidth && i.FractionWidth == FractionWidth;
            }
            return false;
        }

        public override int GetHashCode() {
            HashCode hash = new HashCode();
            hash.Add(Type);
            hash.Add(Constant);
            hash.Add(Volatile);
            hash.Add(Atomic);
            hash.Add(WholeWidth);
            hash.Add(FractionWidth);
            return hash.ToHashCode();
        }

        public override string ToString() {
            return base.ToString() + "fix" + WholeWidth + "x" + FractionWidth;
        }

        public override Expression DefaultValue() {
            throw new NotImplementedException();
        }

    }

}