using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Add numbers or pointers.
    public class OperatorAdd : BinaryMath {

        // Make a new addition operator.
        public OperatorAdd(Expression left, Expression right) : base(left, right, "Add", true) {}

        protected override LLVMValueRef CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {

            // Get arguments.
            LLVMValueRef left = Args[0].CompileRValue(mod, builder, param);
            LLVMValueRef right = Args[1].CompileRValue(mod, builder, param);

            // Compile build.
            VarType retType = Args[0].ReturnType();
            if (retType.IsFloatingPoint()) {
                return builder.BuildFAdd(left, right);
            } else if (retType.Type == VarTypeEnum.Pointer) {
                if (!LeftNumeric) {
                    return builder.BuildGEP(left, new LLVMValueRef[] { right });
                } else {
                    return builder.BuildGEP(right, new LLVMValueRef[] { left });
                }
            } else {
                return builder.BuildAdd(left, right);
            }

        }

    }

}