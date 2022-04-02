using System.Collections.Generic;
using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Operator that can take in a varying number of arguments. By default every operator is overloadable. If an operator function doesn't exist, then the default implementation is used.
    public abstract class NewOperator : Expression {
        public List<Expression> Args;
        public Operator Op;

        // Generic operator constructor.
        public NewOperator(List<Expression> args, Operator op, bool lValue) {
            Type = ExpressionType.Operator;
            Args = args;
            Op = op;
            LValue = lValue;
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

        public abstract void ResolveTypesDefault(); // No overload found, so resolve types.
        public abstract VarType GetReturnTypeDefault(); // No overload found, so get proper return type.
        public abstract ReturnValue CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param); // No overload found, so compile properly.

    }

}