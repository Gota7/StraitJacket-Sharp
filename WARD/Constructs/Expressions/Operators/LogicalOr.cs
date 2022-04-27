using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Logically or two booleans.
    public class OperatorLogicalOr : BooleanMath {

        // Only false if both inputs are false.
        public OperatorLogicalOr(Expression left, Expression right) : base("LogicalOr", left, right) {}

        protected override LLVMValueRef CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            return builder.BuildOr(Args[0].CompileRValue(mod, builder, param), Args[1].CompileRValue(mod, builder, param));
        }

    }

}