using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Negatize numbers.
    public class OperatorNeg : UnaryMath {

        // Make a new negative operator.
        public OperatorNeg(Expression expr) : base(expr, "Neg", false) {}

        protected override LLVMValueRef CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {

            // Get arguments.
            LLVMValueRef expr = Args[0].CompileRValue(mod, builder, param);

            // Compile build.
            VarType retType = Args[0].ReturnType();
            if (retType.IsFloatingPoint()) {
                return builder.BuildFNeg(expr);
            } else {
                return builder.BuildNeg(expr);
            }

        }

    }

}