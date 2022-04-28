using System.Collections.Generic;
using LLVMSharp;
using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Constant integer.
    public class ExpressionConstSimple : Expression {
        public SimplePrimitives Primitive;
        public object Val;

        public ExpressionConstSimple(SimplePrimitives primitive, object val) {
            Type = ExpressionType.ConstSimple;
            Primitive = primitive;
            Val = val;
        }

        public override void ResolveTypes(VarType preferredReturnType, List<VarType> parameterTypes) {
            LValue = false;
        }

        public override VarType GetReturnType() {
            return new VarTypeSimplePrimitive(Primitive);
        }

        public override LLVMValueRef Compile(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            throw new System.NotImplementedException("No defined constant value for simple primitive type: " + Primitive + "!");
        }

        public override string ToString() {
            return "(" + Val.ToString() + ")";
        }

    }

}