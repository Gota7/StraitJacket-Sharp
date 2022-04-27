using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Logically nor two booleans.
    public class OperatorLogicalNor : BooleanMath {

        // Only true if both inputs are false.
        public OperatorLogicalNor(Expression left, Expression right) : base("LogicalNor", left, right) {}

        protected override LLVMValueRef CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            return builder.BuildNot(
                builder.BuildOr(Args[0].CompileRValue(mod, builder, param), Args[1].CompileRValue(mod, builder, param))
            );
        }

    }

}