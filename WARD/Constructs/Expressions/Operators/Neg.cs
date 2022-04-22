using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Negatize numbers.
    public class OperatorNeg : UnaryMath {

        // Make a new negative operator.
        public OperatorNeg(Expression expr) : base(expr, "Neg", false) {}

        protected override ReturnValue CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {

            // Get arguments.
            LLVMValueRef expr = Args[0].Compile(mod, builder, param).Val;
            if (Args[0].LValue) expr = builder.BuildLoad(expr, "tmpLoad");

            // Compile build.
            VarType retType = Args[0].ReturnType();
            if (retType.IsFloatingPoint()) {
                return new ReturnValue(builder.BuildFNeg(expr));
            } else {
                return new ReturnValue(builder.BuildNeg(expr));
            }

        }

    }

}