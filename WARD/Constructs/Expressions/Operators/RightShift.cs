using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Right shift a number.
    public class OperatorRightShift : BitwiseMath {

        // Make a new right shift operator.
        public OperatorRightShift(Expression left, Expression right) : base("RightShift", left, right) {}

        protected override LLVMValueRef CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            LLVMValueRef left = Args[0].CompileRValue(mod, builder, param);
            LLVMValueRef right = Args[1].CompileRValue(mod, builder, param);
            return builder.BuildLShr(left, right);
        }

    }

}