using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Decrement numbers.
    public class OperatorDec : UnaryMath {

        // Make a new decrement operator.
        public OperatorDec(Expression expr) : base(expr, "Dec", true) {}

        protected override ReturnValue CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {

            // Get arguments.
            LLVMValueRef expr = Args[0].Compile(mod, builder, param).Val;
            if (Args[0].LValue) expr = builder.BuildLoad(expr, "tmpLoad");

            // Compile build.
            VarType retType = Args[0].ReturnType();
            if (retType.Type == VarTypeEnum.Pointer) {
                return new ReturnValue(builder.BuildGEP(expr, new LLVMValueRef[] { LLVMValueRef.CreateConstInt(LLVMTypeRef.Int32, ulong.MaxValue, true) }));
            } else if (retType.IsFloatingPoint()) {
                return new ReturnValue(builder.BuildFAdd(expr, LLVMValueRef.CreateConstReal(retType.GetLLVMType(), -1)));
            } else {
                return new ReturnValue(builder.BuildAdd(expr, LLVMValueRef.CreateConstInt(retType.GetLLVMType(), ulong.MaxValue, true)));
            }

        }

    }

}