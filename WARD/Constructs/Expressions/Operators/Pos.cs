using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Positate numbers.
    public class OperatorPos : UnaryMath {

        // Make a new positive operator.
        public OperatorPos(Expression expr) : base(expr, "Pos", false) {}

        protected override LLVMValueRef CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {

            // Get arguments.
            LLVMValueRef expr = Args[0].CompileRValue(mod, builder, param);
            return expr; // Nothing fancy, just return the value for Pos.

        }

    }

}