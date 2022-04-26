using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Unary address of operator.
    public class OperatorAddressOf : OperatorBase {

        // Create a new address of operator.
        public OperatorAddressOf(Expression expr) : base("AddressOf", expr) {}

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

        protected override LLVMValueRef CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            return Args[0].CompileLValue(mod, builder, param); // Is just the L-value so don't load.
        }

    }

}