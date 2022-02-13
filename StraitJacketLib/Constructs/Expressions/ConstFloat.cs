using System.Collections.Generic;
using LLVMSharp;
using LLVMSharp.Interop;

namespace StraitJacketLib.Constructs {

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

        public override void ResolveTypes() {
            LValue = false;
        }

        public override VarType GetReturnType() {
            return new VarTypeFloating(BitWidth);
        }
        
        public override bool IsPlural() {
            return false;
        }

        public override void StoreSingle(ReturnValue src, ReturnValue dest, VarType srcType, VarType destType, LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            throw new System.Exception("??????");
        }

        public override void StorePlural(ReturnValue src, ReturnValue dest, VarType srcType, VarType destType, LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            throw new System.Exception("??????");
        }

        public override ReturnValue Compile(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            return new ReturnValue(LLVMValueRef.CreateConstReal(GetReturnType().GetLLVMType(), Val.ValueDecimal));
        }

        public override string ToString() {
            return Val.ValueDecimal + "f";
        }

    }

}