using System.Collections.Generic;
using LLVMSharp;
using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Constant fixed point.
    public class ExpressionConstFixed : Expression {
        public Number Val;

        public ExpressionConstFixed(long val, int numDecimalPlaces) {
            Type = ExpressionType.ConstFixed;
            Val = new Number() {
                Type = NumberType.Fixed,
                ForceSigned = true,
                ValueWhole = val,
                ValueDecimal = numDecimalPlaces
            };
            ToString();
        }

        public override void ResolveTypes(VarType preferredReturnType, List<VarType> parameterTypes) {
            LValue = false;
        }

        public override VarType GetReturnType() {
            uint wholeWidth = Val.MinBits;
            return new VarTypeFixed(wholeWidth - (uint)Val.ValueDecimal, (uint)Val.ValueDecimal);
        }

        public override LLVMValueRef Compile(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            return LLVMValueRef.CreateConstInt(ReturnType().GetLLVMType(), (ulong)Val.ValueWhole, true);
        }

        public override string ToString() {
            ulong decimalMask = 0;
            for (int i = 0; i < (int)Val.ValueDecimal; i++) {
                decimalMask = decimalMask | (ulong)(long)(0b1 << i);
            }
            ulong wholePortion = (ulong)Val.ValueWhole >> (int)Val.ValueDecimal;
            ulong decPortion = (ulong)Val.ValueWhole & decimalMask;
            double val = wholePortion + (double)decPortion / (1 << (int)Val.ValueDecimal);
            return val + "x";
        }

    }

}