using System.Collections.Generic;
using LLVMSharp;
using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Cast an expression to another type of expression.
    public class ExpressionCast : Expression {
        public Expression ToCast;
        public VarType DestType;
        VarType SrcType;

        public ExpressionCast(Expression toCast, VarType destType) {
            Type = ExpressionType.Cast;
            ToCast = toCast;
            DestType = destType;
        }

        public override void ResolveVariables() {
            ToCast.ResolveVariables();
        }

        public override void ResolveTypes(VarType preferredReturnType, List<VarType> parameterTypes) {
            ToCast.ResolveTypes(preferredReturnType, parameterTypes);
            LValue = false;
            SrcType = ToCast.ReturnType();
            if (!SrcType.CanCastTo(DestType)) {
                throw new System.Exception("BAD CAST!!!");
            }
        }

        public override VarType GetReturnType() {
            return DestType;
        }

        // Compile the cast.
        public override LLVMValueRef Compile(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            LLVMValueRef toCast = ToCast.CompileRValue(mod, builder, param);
            return SrcType.CastTo(toCast, DestType, mod, builder);
        }

        public override string ToString() {
            return "((" + DestType + ")" + ToCast.ToString() + ")";
        }

    }

}