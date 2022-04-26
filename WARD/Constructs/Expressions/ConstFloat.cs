using System.Collections.Generic;
using LLVMSharp;
using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Constant floating point.
    public class ExpressionConstFloat : Expression {
        public Number Val;
        public uint BitWidth;

        public ExpressionConstFloat(uint bitWidth, double val) {
            Type = ExpressionType.ConstFloat;
            Val = new Number() {
                Type = NumberType.Decimal,
                ValueDecimal = val
            };
            BitWidth = bitWidth;
        }

        public override void ResolveTypes(VarType preferredReturnType, List<VarType> parameterTypes) {
            LValue = false;
        }

        public override VarType GetReturnType() {
            return new VarTypeFloating(BitWidth);
        }

        public override LLVMValueRef Compile(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            return LLVMValueRef.CreateConstReal(ReturnType().GetLLVMType(), Val.ValueDecimal);
        }

        public override string ToString() {
            return Val.ValueDecimal + "f";
        }

    }

}