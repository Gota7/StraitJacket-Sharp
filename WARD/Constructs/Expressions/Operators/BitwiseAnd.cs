using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Bitwise and.
    public class OperatorBitwiseAnd : BitwiseMath {

        // Bitwise and operation.
        public OperatorBitwiseAnd(Expression left, Expression right) : base("BitwiseAnd", left, right) {}

        protected override LLVMValueRef CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            return builder.BuildAnd(Args[0].CompileRValue(mod, builder, param), Args[1].CompileRValue(mod, builder, param));
        }

    }

}