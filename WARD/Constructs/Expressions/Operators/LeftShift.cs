using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Left shift a number.
    public class OperatorLeftShift : BitwiseMath {

        // Make a new left shift operator.
        public OperatorLeftShift(Expression left, Expression right) : base("LeftShift", left, right) {}

        protected override LLVMValueRef CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            LLVMValueRef left = Args[0].CompileRValue(mod, builder, param);
            LLVMValueRef right = Args[1].CompileRValue(mod, builder, param);
            return builder.BuildShl(left, right);
        }

    }

}