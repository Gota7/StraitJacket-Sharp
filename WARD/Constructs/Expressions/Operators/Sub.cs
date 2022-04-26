using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Subtract numbers or pointers.
    public class OperatorSub : BinaryMath {

        // Make a new subtraction operator.
        public OperatorSub(Expression left, Expression right) : base(left, right, "Sub", true) {}

        protected override LLVMValueRef CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {

            // Get arguments.
            LLVMValueRef left = Args[0].CompileRValue(mod, builder, param);
            LLVMValueRef right = Args[1].CompileRValue(mod, builder, param);

            // Compile build.
            VarType retType = Args[0].ReturnType();
            if (retType.IsFloatingPoint()) {
                return builder.BuildFSub(left, right);
            } else if (retType.Type == VarTypeEnum.Pointer) {
                if (!LeftNumeric) {
                    return builder.BuildGEP(left, new LLVMValueRef[] { builder.BuildNeg(right) });
                } else {
                    return builder.BuildGEP(right, new LLVMValueRef[] { builder.BuildNeg(left) });
                }
            } else {
                return builder.BuildSub(left, right);
            }

        }

    }

}