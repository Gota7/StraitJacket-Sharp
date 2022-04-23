using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Subtract numbers or pointers.
    public class OperatorSub : BinaryMath {

        // Make a new subtraction operator.
        public OperatorSub(Expression left, Expression right) : base(left, right, "Sub", true) {}

        protected override ReturnValue CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {

            // Get arguments.
            LLVMValueRef left = Args[0].Compile(mod, builder, param).Val;
            if (Args[0].LValue) left = builder.BuildLoad(left, "tmpLoad");
            LLVMValueRef right = Args[1].Compile(mod, builder, param).Val;
            if (Args[1].LValue) right = builder.BuildLoad(right, "tmpLoad");

            // Compile build.
            VarType retType = Args[0].ReturnType();
            if (retType.IsFloatingPoint()) {
                return new ReturnValue(builder.BuildFSub(left, right));
            } else if (retType.Type == VarTypeEnum.Pointer) {
                if (!LeftNumeric) {
                    return new ReturnValue(builder.BuildGEP(left, new LLVMValueRef[] { builder.BuildNeg(right) }));
                } else {
                    return new ReturnValue(builder.BuildGEP(right, new LLVMValueRef[] { builder.BuildNeg(left) }));
                }
            } else {
                return new ReturnValue(builder.BuildSub(left, right));
            }

        }

    }

}