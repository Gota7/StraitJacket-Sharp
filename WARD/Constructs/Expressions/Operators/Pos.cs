using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Positate numbers.
    public class OperatorPos : UnaryMath {

        // Make a new positive operator.
        public OperatorPos(Expression expr) : base(expr, "Pos", false) {}

        protected override ReturnValue CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {

            // Get arguments.
            LLVMValueRef expr = Args[0].Compile(mod, builder, param).Val;
            if (Args[0].LValue) expr = builder.BuildLoad(expr, "tmpLoad");
            return new ReturnValue(expr); // Nothing fancy, just return the value for Pos.

        }

    }

}