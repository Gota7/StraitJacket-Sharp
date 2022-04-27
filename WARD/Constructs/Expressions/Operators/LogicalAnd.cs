using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Logically and two booleans.
    public class OperatorLogicalAnd : BooleanMath {

        // Only true if both inputs are true.
        public OperatorLogicalAnd(Expression left, Expression right) : base("LogicalAnd", left, right) {}

        protected override LLVMValueRef CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            return builder.BuildAnd(Args[0].CompileRValue(mod, builder, param), Args[1].CompileRValue(mod, builder, param));
        }

    }

}