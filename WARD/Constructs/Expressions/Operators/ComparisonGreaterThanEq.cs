using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Greater than or equal to comparison.
    public class OperatorGreaterThanEq : BinaryComparison {

        // Greater than or equal to comparison.
        public OperatorGreaterThanEq(Expression left, Expression right) : base(left, right, "Ge") {}

        protected override LLVMValueRef CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            if (Floating) {
                return builder.BuildFCmp(
                    LLVMRealPredicate.LLVMRealOGE,
                    Args[0].CompileRValue(mod, builder, param),
                    Args[1].CompileRValue(mod, builder, param)
                );
            } else if (Signed) {
                return builder.BuildICmp(
                    LLVMIntPredicate.LLVMIntSGE,
                    Args[0].CompileRValue(mod, builder, param),
                    Args[1].CompileRValue(mod, builder, param)
                );
            } else {
                return builder.BuildICmp(
                    LLVMIntPredicate.LLVMIntUGE,
                    Args[0].CompileRValue(mod, builder, param),
                    Args[1].CompileRValue(mod, builder, param)
                );
            }
        }

    }

}