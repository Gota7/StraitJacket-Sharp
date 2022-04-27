using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Bitwise nand.
    public class OperatorBitwiseNand : BitwiseMath {

        // Bitwise nand operation.
        public OperatorBitwiseNand(Expression left, Expression right) : base("BitwiseNand", left, right) {}

        protected override LLVMValueRef CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            return builder.BuildNot(
                builder.BuildAnd(Args[0].CompileRValue(mod, builder, param), Args[1].CompileRValue(mod, builder, param))
            );
        }

    }

}