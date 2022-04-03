using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Multiply numbers or pointers.
    public class OperatorMul : OperatorMath {

        // Make a new multiplication operator.
        public OperatorMul(Expression left, Expression right) : base(left, right, "Mul", true) {}

        protected override ReturnValue CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {

            // Get arguments.
            LLVMValueRef left = Args[0].Compile(mod, builder, param).Val;
            if (Args[0].LValue) left = builder.BuildLoad(left, "tmpLoad");
            LLVMValueRef right = Args[1].Compile(mod, builder, param).Val;
            if (Args[1].LValue) right = builder.BuildLoad(right, "tmpLoad");

            // Compile build.
            VarType retType = Args[0].ReturnType();
            if (retType.IsFloatingPoint()) {
                return new ReturnValue(builder.BuildFMul(left, right));
            } else {
                return new ReturnValue(builder.BuildMul(left, right));
            }

        }

    }

}