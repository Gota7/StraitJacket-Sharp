using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Bitwise xor.
    public class OperatorBitwiseXor : BitwiseMath {

        // Bitwise xor operation.
        public OperatorBitwiseXor(Expression left, Expression right) : base("BitwiseXor", left, right) {}

        protected override LLVMValueRef CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            return builder.BuildXor(Args[0].CompileRValue(mod, builder, param), Args[1].CompileRValue(mod, builder, param));
        }

    }

}