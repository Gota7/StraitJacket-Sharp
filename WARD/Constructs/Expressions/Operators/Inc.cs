using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Increment numbers.
    public class OperatorInc : UnaryMath {

        // Make a new increment operator. TODO: POSTFIX RETURNS ORIGINAL ITEM, PREFIX RETURNS LVALUE REFERENCE TO VAR!!! DO LVALUE CHECKS!!
        public OperatorInc(Expression expr) : base(expr, "Inc", true) {}

        protected override ReturnValue CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {

            // Get arguments.
            LLVMValueRef expr = Args[0].Compile(mod, builder, param).Val;
            if (Args[0].LValue) expr = builder.BuildLoad(expr, "tmpLoad");

            // Compile build.
            VarType retType = Args[0].ReturnType();
            if (retType.Type == VarTypeEnum.Pointer) {
                return new ReturnValue(builder.BuildGEP(expr, new LLVMValueRef[] { LLVMValueRef.CreateConstInt(LLVMTypeRef.Int32, 1, true) }));
            } else if (retType.IsFloatingPoint()) {
                return new ReturnValue(builder.BuildFAdd(expr, LLVMValueRef.CreateConstReal(retType.GetLLVMType(), 1)));
            } else {
                return new ReturnValue(builder.BuildAdd(expr, LLVMValueRef.CreateConstInt(retType.GetLLVMType(), 1)));
            }

        }

    }

}