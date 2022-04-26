using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Divide numbers.
    public class OperatorDiv : BinaryMath {

        // Make a new division operator.
        public OperatorDiv(Expression left, Expression right) : base(left, right, "Div", false) {}

        protected override LLVMValueRef CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {

            // Get arguments.
            LLVMValueRef left = Args[0].CompileRValue(mod, builder, param);
            LLVMValueRef right = Args[1].CompileRValue(mod, builder, param);

            // Compile build.
            VarType retType = Args[0].ReturnType();
            if (retType.IsFloatingPoint()) {
                return builder.BuildFDiv(left, right);
            } else {
                if (retType.IsUnsigned()) {
                    return builder.BuildUDiv(left, right);
                } else {
                    return builder.BuildSDiv(left, right);
                }
            }

        }

    }

}