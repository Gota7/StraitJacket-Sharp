using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Logically xor two booleans.
    public class OperatorLogicalXor : BooleanMath {

        // Only true if both operands are different.
        public OperatorLogicalXor(Expression left, Expression right) : base("LogicalXor", left, right) {}

        protected override LLVMValueRef CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            return builder.BuildXor(Args[0].CompileRValue(mod, builder, param), Args[1].CompileRValue(mod, builder, param));
        }

    }

}