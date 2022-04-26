using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Modulo numbers.
    public class OperatorMod : BinaryMath {

        // Make a new modulo operator.
        public OperatorMod(Expression left, Expression right) : base(left, right, "Mod", false) {}

        protected override LLVMValueRef CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {

            // Get arguments.
            LLVMValueRef left = Args[0].CompileRValue(mod, builder, param);
            LLVMValueRef right = Args[1].CompileRValue(mod, builder, param);

            // Compile build.
            VarType retType = Args[0].ReturnType();
            if (retType.IsFloatingPoint()) {
                return builder.BuildFRem(left, right);
            } else {
                if (retType.IsUnsigned()) {
                    return builder.BuildURem(left, right);
                } else {
                    return builder.BuildSRem(left, right);
                }
            }

        }

    }

}