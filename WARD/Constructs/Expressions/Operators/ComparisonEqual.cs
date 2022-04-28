using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Equal to comparison.
    public class OperatorEqualTo : BinaryComparison {

        // Equal to comparison.
        public OperatorEqualTo(Expression left, Expression right) : base(left, right, "Eq") {}

        protected override LLVMValueRef CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            if (Floating) {
                return builder.BuildFCmp(
                    LLVMRealPredicate.LLVMRealOEQ,
                    Args[0].CompileRValue(mod, builder, param),
                    Args[1].CompileRValue(mod, builder, param)
                );
            } else {
                return builder.BuildICmp(
                    LLVMIntPredicate.LLVMIntEQ,
                    Args[0].CompileRValue(mod, builder, param),
                    Args[1].CompileRValue(mod, builder, param)
                );
            }
        }

    }

}