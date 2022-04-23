using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Add numbers or pointers.
    public class OperatorAdd : BinaryMath {

        // Make a new addition operator.
        public OperatorAdd(Expression left, Expression right) : base(left, right, "Add", true) {}

        protected override ReturnValue CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {

            // Get arguments.
            LLVMValueRef left = Args[0].Compile(mod, builder, param).Val;
            if (Args[0].LValue) left = builder.BuildLoad(left, "tmpLoad");
            LLVMValueRef right = Args[1].Compile(mod, builder, param).Val;
            if (Args[1].LValue) right = builder.BuildLoad(right, "tmpLoad");

            // Compile build.
            VarType retType = Args[0].ReturnType();
            if (retType.IsFloatingPoint()) {
                return new ReturnValue(builder.BuildFAdd(left, right));
            } else if (retType.Type == VarTypeEnum.Pointer) {
                if (!LeftNumeric) {
                    return new ReturnValue(builder.BuildGEP(left, new LLVMValueRef[] { right }));
                } else {
                    return new ReturnValue(builder.BuildGEP(right, new LLVMValueRef[] { left }));
                }
            } else {
                return new ReturnValue(builder.BuildAdd(left, right));
            }

        }

    }

}