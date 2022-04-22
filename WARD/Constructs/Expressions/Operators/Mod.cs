using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Modulo numbers.
    public class OperatorMod : BinaryMath {

        // Make a new modulo operator.
        public OperatorMod(Expression left, Expression right) : base(left, right, "Mod", false) {}

        protected override ReturnValue CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {

            // Get arguments.
            LLVMValueRef left = Args[0].Compile(mod, builder, param).Val;
            if (Args[0].LValue) left = builder.BuildLoad(left, "tmpLoad");
            LLVMValueRef right = Args[1].Compile(mod, builder, param).Val;
            if (Args[1].LValue) right = builder.BuildLoad(right, "tmpLoad");

            // Compile build.
            VarType retType = Args[0].ReturnType();
            if (retType.IsFloatingPoint()) {
                return new ReturnValue(builder.BuildFRem(left, right));
            } else {
                if (retType.IsUnsigned()) {
                    return new ReturnValue(builder.BuildURem(left, right));
                } else {
                    return new ReturnValue(builder.BuildSRem(left, right));
                }
            }

        }

    }

}