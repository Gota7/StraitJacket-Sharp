using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Bitwise or.
    public class OperatorBitwiseOr : BitwiseMath {

        // Bitwise or operation.
        public OperatorBitwiseOr(Expression left, Expression right) : base("BitwiseOr", left, right) {}

        protected override LLVMValueRef CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            return builder.BuildOr(Args[0].CompileRValue(mod, builder, param), Args[1].CompileRValue(mod, builder, param));
        }

    }

}