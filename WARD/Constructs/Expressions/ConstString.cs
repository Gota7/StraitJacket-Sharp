using System.Collections.Generic;
using LLVMSharp;
using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Constant string pointer.
    public class ExpressionConstStringPtr : Expression {
        public string Str;

        public ExpressionConstStringPtr(string str) {
            Type = ExpressionType.ConstString;
            Str = str;
        }

        public override void ResolveTypes(VarType preferredReturnType, List<VarType> parameterTypes) {
            LValue = false;
        }

        public override VarType GetReturnType() {
            return new VarTypeSimplePrimitive(SimplePrimitives.ConstString);
        }

        public override LLVMValueRef Compile(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            return builder.BuildGlobalStringPtr(Str, "SJ_ConstStr_" + Str);
        }

        public override string ToString() {
            return "\"" + Str.ToString()+ "\"";
        }

    }

}