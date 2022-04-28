using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Less than or equal to comparison.
    public class OperatorLessThanEq : BinaryComparison {

        // Less than or equal to comparison.
        public OperatorLessThanEq(Expression left, Expression right) : base(left, right, "Le") {}

        protected override LLVMValueRef CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            if (Floating) {
                return builder.BuildFCmp(
                    LLVMRealPredicate.LLVMRealOLE,
                    Args[0].CompileRValue(mod, builder, param),
                    Args[1].CompileRValue(mod, builder, param)
                );
            } else if (Signed) {
                return builder.BuildICmp(
                    LLVMIntPredicate.LLVMIntSLE,
                    Args[0].CompileRValue(mod, builder, param),
                    Args[1].CompileRValue(mod, builder, param)
                );
            } else {
                return builder.BuildICmp(
                    LLVMIntPredicate.LLVMIntULE,
                    Args[0].CompileRValue(mod, builder, param),
                    Args[1].CompileRValue(mod, builder, param)
                );
            }
        }

    }

}