using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Logically nand two booleans.
    public class OperatorLogicalNand : BooleanMath {

        // Only false if both inputs are true.
        public OperatorLogicalNand(Expression left, Expression right) : base("LogicalNand", left, right) {}

        protected override LLVMValueRef CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            return builder.BuildNot(
                builder.BuildAnd(Args[0].CompileRValue(mod, builder, param), Args[1].CompileRValue(mod, builder, param))
            );
        }

    }

}