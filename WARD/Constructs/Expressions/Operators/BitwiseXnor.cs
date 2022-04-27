using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Bitwise xnor.
    public class OperatorBitwiseXnor : BitwiseMath {

        // Bitwise xnor operation.
        public OperatorBitwiseXnor(Expression left, Expression right) : base("BitwiseXnor", left, right) {}

        protected override LLVMValueRef CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            return builder.BuildNot(
                builder.BuildXor(Args[0].CompileRValue(mod, builder, param), Args[1].CompileRValue(mod, builder, param))
            );
        }

    }

}