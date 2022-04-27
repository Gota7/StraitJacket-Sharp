using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Bitwise nor.
    public class OperatorBitwiseNor : BitwiseMath {

        // Bitwise nor operation.
        public OperatorBitwiseNor(Expression left, Expression right) : base("BitwiseNor", left, right) {}

        protected override LLVMValueRef CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            return builder.BuildNot(
                builder.BuildOr(Args[0].CompileRValue(mod, builder, param), Args[1].CompileRValue(mod, builder, param))
            );
        }

    }

}