using System.Collections.Generic;
using LLVMSharp;
using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Store a value somewhere. Operates different depending on how many values are returned.
    public class ExpressionStore : Expression {
        public Expression Src;
        public Expression Dest;

        public ExpressionStore(Expression src, Expression dest) {
            Type = ExpressionType.Store;
            Src = src;
            Dest = dest;
        }

        public override void ResolveVariables() {
            Src.ResolveVariables();
            Dest.ResolveVariables();
        }

        public override void ResolveTypes(VarType preferredReturnType, List<VarType> parameterTypes) {

            // Resolve types.
            LValue = false;
            Src.ResolveTypes();
            Dest.ResolveTypes(Src.ReturnType(), null);

            // Exact match.
            if (Src.ReturnType().Equals(Dest.ReturnType())) {
                // Good!
            }

            // Add an implicit cast.
            else if (Src.ReturnType().CanImplicitlyCastTo(Dest.ReturnType())) {
                ExpressionCast cast = new ExpressionCast(Src, Dest.ReturnType());
                Src = cast;
                Src.ResolveTypes();
            }

            // Can not convert...
            else {
                throw new System.Exception("NONMATCHING RETURN TYPES!!!");
            }

        }

        // A store operation has no return type.
        public override VarType GetReturnType() {
            return new VarTypeSimplePrimitive(SimplePrimitives.Void);
        }

        // Compile the store, but return nothing.
        public override LLVMValueRef Compile(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {

            // Get the values from the source and destination.
            LLVMValueRef dest = Dest.CompileLValue(mod, builder, param);
            if (dest == null) throw new System.Exception("Can't store to a non-lvalue!");
            builder.BuildStore(Src.CompileRValue(mod, builder, param), dest);

            // Finish the return.
            return null;

        }

        public override string ToString() {
            return "(" + Dest.ToString() + " = " + Src.ToString() + ")";
        }

    }

}