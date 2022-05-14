using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Increment numbers.
    public class OperatorInc : UnaryMath {
        public bool IsPrefix; // If this is to be ran as a prefix operator or a postfix operator.

        // Make a new increment operator. Postfix returns original item, prefix returns L-value reference to var.
        public OperatorInc(Expression expr, bool isPrefix = false) : base(expr, "Inc", true) {
            IsPrefix = isPrefix;
        }

        // Override L-value.
        protected override void ResolveTypesDefault() {
            base.ResolveTypesDefault();
            LValue = IsPrefix;
        }

        protected override LLVMValueRef CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {

            // Prefix mode.
            if (IsPrefix) {

                // Get arguments.
                LLVMValueRef expr = Args[0].CompileLValue(mod, builder, param);

                // Compile build. TODO: DO STORE!!!
                /*VarType retType = Args[0].ReturnType();
                if (retType.Type == VarTypeEnum.Pointer) {
                    builder.BuildGEP(expr, new LLVMValueRef[] { LLVMValueRef.CreateConstInt(LLVMTypeRef.Int32, 1, true) });
                } else if (retType.IsFloatingPoint()) {
                    builder.BuildFAdd(expr, LLVMValueRef.CreateConstReal(retType.GetLLVMType(), 1));
                } else {
                    builder.BuildAdd(expr, LLVMValueRef.CreateConstInt(retType.GetLLVMType(), 1));
                }*/
                return expr;

            } else {

                // Get arguments.
                LLVMValueRef expr = Args[0].CompileRValue(mod, builder, param);

                // Compile build.
                VarType retType = Args[0].ReturnType();
                if (retType.Type == VarTypeEnum.Pointer) {
                    return builder.BuildGEP(expr, new LLVMValueRef[] { LLVMValueRef.CreateConstInt(LLVMTypeRef.Int32, 1, true) });
                } else if (retType.IsFloatingPoint()) {
                    return builder.BuildFAdd(expr, LLVMValueRef.CreateConstReal(retType.GetLLVMType(), 1));
                } else {
                    return builder.BuildAdd(expr, LLVMValueRef.CreateConstInt(retType.GetLLVMType(), 1));
                }

            }

        }

    }

}