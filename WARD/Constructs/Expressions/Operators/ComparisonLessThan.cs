using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Less than comparison.
    public class OperatorLessThan : BinaryComparison {

        // Less than comparison.
        public OperatorLessThan(Expression left, Expression right) : base(left, right, "Lt") {}

        protected override LLVMValueRef CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            if (Floating) {
                return builder.BuildFCmp(
                    LLVMRealPredicate.LLVMRealOLT,
                    Args[0].CompileRValue(mod, builder, param),
                    Args[1].CompileRValue(mod, builder, param)
                );
            } else if (Signed) {
                return builder.BuildICmp(
                    LLVMIntPredicate.LLVMIntSLT,
                    Args[0].CompileRValue(mod, builder, param),
                    Args[1].CompileRValue(mod, builder, param)
                );
            } else {
                return builder.BuildICmp(
                    LLVMIntPredicate.LLVMIntULT,
                    Args[0].CompileRValue(mod, builder, param),
                    Args[1].CompileRValue(mod, builder, param)
                );
            }
        }

    }

}