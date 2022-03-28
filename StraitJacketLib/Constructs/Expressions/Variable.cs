using System.Collections.Generic;
using LLVMSharp;
using LLVMSharp.Interop;

namespace StraitJacketLib.Constructs {

    // A variable to resolve.
    public class ExpressionVariable : Expression {
        public VariableOrFunction ToResolve;
        List<Variable> PossibleReturns;
        Variable Resolved;

        public ExpressionVariable(VariableOrFunction toResolve) {
            Type = ExpressionType.Variable;
            ToResolve = toResolve;
        }

        public override void ResolveVariables() {
            PossibleReturns = ToResolve.ResolveVariable();
        }

        public override void ResolveTypes(VarType preferredReturnType, List<VarType> parameterTypes) {

            // So if the parameter types are not null, this means we are looking for a function pointer or a function to call.
            if (parameterTypes != null) {

                // Get the items that are variables and not functions.
                List<Variable> possibleVars = new List<Variable>();
                foreach (var v in PossibleReturns) {
                    if (v as Function == null && v.Type.Type == VarTypeEnum.PrimitiveFunction) {
                        VarTypeFunction funcType = v.Type as VarTypeFunction;
                        if (funcType.CanBeConvertedFrom(parameterTypes, preferredReturnType)) possibleVars.Add(v);
                    }
                }

                // Manage possible variables.
                if (possibleVars.Count == 1) {
                    Resolved = possibleVars[0];
                    return;
                } else if (possibleVars.Count > 1) {
                    throw new System.Exception("Can't resolve variable!!!");
                }
            
                // This is a function call, find the proper item.
                LValue = false;
                Resolved = ToResolve.ResolveFunctionVariable(parameterTypes, preferredReturnType);
                (Resolved.Type as VarTypeFunction).FoundFunc = (Function)Resolved; // Hack for call expressions.
                
            } else {

                // The resolved variable should just be the only possible one.
                if (PossibleReturns.Count == 1) {
                    Resolved = PossibleReturns[0];
                    return;
                } else if (PossibleReturns.Count > 1) {
                    throw new System.Exception("Can't resolve variable!!!");
                }

            }

            // Unknown type, forbid loading.
            if (Resolved.Type == null) {
                LValue = false;
            }

        }

        public Variable GetResolved => Resolved;

        public override VarType GetReturnType() {
            return Resolved.Type;
        }

        public override bool IsPlural() {
            return false;
        }

        public override void StoreSingle(ReturnValue src, ReturnValue dest, VarType srcType, VarType destType, LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            
            // Ex: u32 a = 7;
            // Ex: (u8, (u32, f32)) b = (3, (5, 7));

            // Case 1: Raw value.
            if (src.ReturnType == ReturnValueType.Value) {
                builder.BuildStore(src.Val, dest.Val);
            }

            // Case 2: Recursive multiple values (tuple). We are storing withing a tuple.
            else if (src.ReturnType == ReturnValueType.NestedValues) {
                // TODO!!!
                throw new System.NotImplementedException();
            }

            // How did we get here?
            else {
                throw new System.Exception("??????");
            }

        }

        public override void StorePlural(ReturnValue src, ReturnValue dest, VarType srcType, VarType destType, LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            throw new System.NotImplementedException();
        }

        public override ReturnValue Compile(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            return new ReturnValue(Resolved.LLVMValue);
        }

        public override string ToString() {
            return ToResolve.ToString();
        }

    }

}