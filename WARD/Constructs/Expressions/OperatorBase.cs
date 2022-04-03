using System.Collections.Generic;
using System.Linq;
using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Operator that can take in a varying number of arguments. By default every operator is overloadable. If an operator function doesn't exist, then the default implementation is used.
    public abstract class OperatorBase : Expression {
        public Expression[] Args;
        public string Op;

        // Generic operator constructor.
        public OperatorBase(string op, params Expression[] args) {
            Type = ExpressionType.Operator;
            Args = args;
            Op = op;
        }

        public override void ResolveVariables() {
            foreach (var e in Args) {
                e.ResolveVariables();
            }
        }

        public override void ResolveTypes(VarType preferredReturnType, List<VarType> parameterTypes) {
            List<VarType> pTypes = new List<VarType>();
            foreach (var e in Args) {
                e.ResolveTypes(preferredReturnType, parameterTypes);
                pTypes.Add(e.ReturnType());
            }
            // TODO: USE PTYPES TO RESOLVE OPERATOR OVERLOAD FUNCTION!!!
        }

        public override VarType GetReturnType() {
            return null; // TODO!!!
        }

        public override ReturnValue Compile(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            return null; // TODO!!!
        }

        public override bool IsPlural() {
            return false;
        }

        public override void StoreSingle(ReturnValue src, ReturnValue dest, VarType srcType, VarType destType, LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            Args[0].StoreSingle(src, dest, srcType, destType, mod, builder, param);
        }

        public override void StorePlural(ReturnValue src, ReturnValue dest, VarType srcType, VarType destType, LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            Args[0].StorePlural(src, dest, srcType, destType, mod, builder, param);
        }

        protected abstract void ResolveTypesDefault(); // No overload found, so resolve types.
        protected abstract VarType GetReturnTypeDefault(); // No overload found, so get proper return type.
        protected abstract ReturnValue CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param); // No overload found, so compile properly.

    }

}