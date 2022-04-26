using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Return statement.
    public class ReturnStatement : ICompileable {
        public Expression ReturnValue;
        public FileContext FileContext;

        public FileContext GetFileContext() => FileContext;

        public ReturnStatement(Expression expr) {
            ReturnValue = expr;
        }

        public void ResolveVariables() {
            if (ReturnValue != null) ReturnValue.ResolveVariables();
        }

        public void ResolveTypes() {
            if (ReturnValue != null) ReturnValue.ResolveTypes();
        }

        public void CompileDeclarations(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {}

        public LLVMValueRef Compile(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {

            // Only compile if not dead.
            if (CodeStatements.BlockTerminated) return null;

            // Return a value.
            if (ReturnValue == null) {
                CodeStatements.BlockTerminated = true;
                CodeStatements.ReturnedValue = builder.BuildRetVoid();
                return CodeStatements.ReturnedValue;
            } else if (ReturnValue.ReturnType().Equals(new VarTypeSimplePrimitive(SimplePrimitives.Void))) {
                CodeStatements.BlockTerminated = true;
                CodeStatements.ReturnedValue = builder.BuildRetVoid();
                return CodeStatements.ReturnedValue;
            } else {
                LLVMValueRef ret = ReturnValue.Compile(mod, builder, param);
                if (ReturnValue.LValue) ret = builder.BuildLoad(ret, "SJ_LoadRet");
                CodeStatements.BlockTerminated = true;
                CodeStatements.ReturnedValue = ret;
                builder.BuildRet(ret);
                return ret;
            }

        }

        public override string ToString() {
            string ret = "return";
            if (ReturnValue != null) ret += " " + ReturnValue.ToString();
            return ret;
        }

    }

}