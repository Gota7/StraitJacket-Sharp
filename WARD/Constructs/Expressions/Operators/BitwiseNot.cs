using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Take the opposite of an item.
    public class OperatorBitwiseNot : BitwiseMath {

        // Take the opposite of a boolean.
        public OperatorBitwiseNot(Expression expr) : base("BitwiseNot", expr) {}

        protected override LLVMValueRef CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            return builder.BuildNot(Args[0].CompileRValue(mod, builder, param));
        }

    }

}