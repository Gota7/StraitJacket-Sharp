using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Greater than comparison.
    public class OperatorGreaterThan : BinaryComparison {

        // Greater than comparison.
        public OperatorGreaterThan(Expression left, Expression right) : base(left, right, "Gt") {}

        protected override LLVMValueRef CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            if (Floating) {
                return builder.BuildFCmp(
                    LLVMRealPredicate.LLVMRealOGT,
                    Args[0].CompileRValue(mod, builder, param),
                    Args[1].CompileRValue(mod, builder, param)
                );
            } else if (Signed) {
                return builder.BuildICmp(
                    LLVMIntPredicate.LLVMIntSGT,
                    Args[0].CompileRValue(mod, builder, param),
                    Args[1].CompileRValue(mod, builder, param)
                );
            } else {
                return builder.BuildICmp(
                    LLVMIntPredicate.LLVMIntUGT,
                    Args[0].CompileRValue(mod, builder, param),
                    Args[1].CompileRValue(mod, builder, param)
                );
            }
        }

    }

}