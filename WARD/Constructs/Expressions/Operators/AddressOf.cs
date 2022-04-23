using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Unary address of operator.
    public class OperatorAddressOf : OperatorBase {

        // Create a new address of operator.
        public OperatorAddressOf(Expression expr, string opName = "AddressOf") : base(opName, expr) {}

        protected override void ResolveTypesDefault() {

            // Make sure argument is an L-value.
            if (!Args[0].LValue) {
                throw new System.Exception("Can't take the address of a non-lvalue!");
            }
            LValue = false;

        }

        protected override VarType GetReturnTypeDefault() {
            return new VarTypePointer(Args[0].ReturnType());
        }

        protected override ReturnValue CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            return Args[0].Compile(mod, builder, param); // Is just the l-value so don't load.
        }

    }

}