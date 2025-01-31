using System.Collections.Generic;
using LLVMSharp;
using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Constant integer.
    public class ExpressionConstInt : Expression {
        public Number Val;

        public ExpressionConstInt(bool forceSigned, long val) {
            Type = ExpressionType.ConstInt;
            Val = new Number() {
                Type = NumberType.Whole,
                ForceSigned = forceSigned,
                ValueWhole = val
            };
        }

        public override void ResolveTypes(VarType preferredReturnType, List<VarType> parameterTypes) {
            LValue = false;
        }

        public override VarType GetReturnType() {
            return new VarTypeInteger(Val.ForceSigned, Val.MinBits);
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
            return new ReturnValue(LLVMValueRef.CreateConstInt(ReturnType().GetLLVMType(), (ulong)Val.ValueWhole, Val.ForceSigned));
        }

        public override string ToString() {
            return Val.ValueWhole.ToString();
        }

    }

}