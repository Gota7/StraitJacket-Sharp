using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Multiply numbers.
    public class OperatorMul : BinaryMath {

        // Make a new multiplication operator.
        public OperatorMul(Expression left, Expression right) : base(left, right, "Mul", false) {}

        protected override LLVMValueRef CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {

            // Get arguments.
            LLVMValueRef left = Args[0].CompileRValue(mod, builder, param);
            LLVMValueRef right = Args[1].CompileRValue(mod, builder, param);

            // Compile build.
            VarType retType = Args[0].ReturnType();
            if (retType.IsFloatingPoint()) {
                return builder.BuildFMul(left, right);
            } else {
                return builder.BuildMul(left, right);
            }

        }

    }

}