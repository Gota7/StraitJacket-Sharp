using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Set operator to store results.
    public class OperatorSet : OperatorBase {

        // Make a new set operator.
        public OperatorSet(Expression left, Expression right) : base("Set", left, right) {}

        protected override void ResolveTypesDefault() {
            if (!Args[0].LValue) throw new System.Exception("Can not assign to a non l-value!");
            if (!Args[1].ReturnType().CanImplicitlyCastTo(Args[0].ReturnType())) throw new System.Exception("Can not assign non-matching types!");
            if (!Args[0].ReturnType().Equals(Args[1].ReturnType())) {
                Args[1] = new ExpressionCast(Args[1], Args[0].ReturnType()); // Cast if needed.
                Args[1].ResolveTypes();
            }
            LValue = true;
        }

        protected override VarType GetReturnTypeDefault() {
            return Args[0].ReturnType();
        }

        protected override LLVMValueRef CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            LLVMValueRef left = Args[0].CompileLValue(mod, builder, param);
            LLVMValueRef right = Args[1].CompileRValue(mod, builder, param);
            builder.BuildStore(right, left);
            return left; // Return L-value to value being stored to.
        }

    }

}