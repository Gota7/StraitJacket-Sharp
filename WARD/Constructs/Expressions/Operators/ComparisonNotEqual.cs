using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Not equal to comparison.
    public class OperatorNotEqualTo : BinaryComparison {

        // Not equal to comparison.
        public OperatorNotEqualTo(Expression left, Expression right) : base(left, right, "Ne") {}

        protected override LLVMValueRef CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            if (Floating) {
                return builder.BuildFCmp(
                    LLVMRealPredicate.LLVMRealONE,
                    Args[0].CompileRValue(mod, builder, param),
                    Args[1].CompileRValue(mod, builder, param)
                );
            } else {
                return builder.BuildICmp(
                    LLVMIntPredicate.LLVMIntNE,
                    Args[0].CompileRValue(mod, builder, param),
                    Args[1].CompileRValue(mod, builder, param)
                );
            }
        }

    }

}