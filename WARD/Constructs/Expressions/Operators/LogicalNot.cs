using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Take the opposite of a boolean.
    public class OperatorLogicalNot : BooleanMath {

        // Take the opposite of a boolean.
        public OperatorLogicalNot(Expression expr) : base("Not", expr) {}

        protected override LLVMValueRef CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            return builder.BuildNot(Args[0].CompileRValue(mod, builder, param));
        }

    }

}