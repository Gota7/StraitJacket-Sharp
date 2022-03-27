using System.Collections.Generic;
using System.Linq;
using LLVMSharp;
using LLVMSharp.Interop;

namespace StraitJacketLib.Constructs {

    // Call the result of a value of a function. TODO: FIX EVERYTHING THIS IS SOOOOO HACKY!
    public class ExpressionCall : Expression {
        public Expression ToCall;
        public ExpressionComma Parameters;

        public ExpressionCall(Expression toCall, ExpressionComma parameters) {
            Type = ExpressionType.Call;
            ToCall = toCall;
            Parameters = parameters;
        }

        public override void ResolveVariables() {
            ToCall.ResolveVariables();
            Parameters.ResolveVariables();
        }

        public override void ResolveTypes(VarType preferredReturnType, List<VarType> parameterTypes) {

            // Get parameter types.
            LValue = false;
            Parameters.ResolveTypes();
            List<VarType> paramTypes = new List<VarType>();
            foreach (var e in Parameters.Expressions) {
                paramTypes.Add(e.ReturnType());
            }

            // Resolve what is to be called.
            ToCall.ResolveTypes(preferredReturnType, paramTypes);

            // Check if function type is compatible.

        }

        public override VarType GetReturnType() {
            return ToCall.ReturnType();
        }

        public override bool IsPlural() {
            return false;
        }

        public override void StoreSingle(ReturnValue src, ReturnValue dest, VarType srcType, VarType destType, LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            throw new System.Exception("??????");
        }

        public override void StorePlural(ReturnValue src, ReturnValue dest, VarType srcType, VarType destType, LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            throw new System.Exception("??????");
        }

        // TODO: TUPLE PARAMETERS!!!
        public override ReturnValue Compile(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {

            // Get function to call.
            Function FunctionToCall = (ToCall.ReturnType() as VarTypeFunction).FoundFunc;

            // LLVM.
            if (FunctionToCall != null && FunctionToCall.Equals(AsyLLVM.Function)) {
                return AsyLLVM.CompileCall(mod, builder, Parameters.Expressions);
            }

            // Compile values.
            ReturnValue toCall = ToCall.Compile(mod, builder, param);
            if (ToCall.LValue) toCall = new ReturnValue(builder.BuildLoad(toCall.Val));

            // Compile parameters.
            ReturnValue parameters = Parameters.Compile(mod, builder, param);
            LLVMValueRef[] args = parameters.Rets.Select(x => x.Val).ToArray(); // TODO: TUPLE PARAMETERS!!!

            // Function pointer.
            if (FunctionToCall == null) {
                return new ReturnValue(builder.BuildCall(toCall.Val, args));
            }

            // Make sure function is compiled.
            if (!FunctionToCall.Compiled) FunctionToCall.Compile(mod, builder, param);

            // Inline function.
            if (FunctionToCall.Inline) {

                // Fix arguments.
                //Scope.PushFunction(FunctionToCall);
                for (int i = 0; i < args.Length; i++) {

                    // Variadic.
                    if (i >= FunctionToCall.Parameters.Count || (i == FunctionToCall.Parameters.Count - 1 && FunctionToCall.Parameters.Last().Value.Type.Variadic)) {
                        FunctionToCall.Parameters.Last().VariadicArgs = new List<LLVMValueRef>();
                        var val = builder.BuildAlloca(args[i].TypeOf, "SJ_Param_Variadic_" + i);
                        builder.BuildStore(args[i], val);
                        FunctionToCall.Parameters.Last().VariadicArgs.Add(val);
                    } else {
                        var val = builder.BuildAlloca(args[i].TypeOf, "SJ_Param_" + FunctionToCall.Parameters[i].Value.Name);
                        builder.BuildStore(args[i], val);
                        FunctionToCall.Parameters[i].Value.LLVMValue = val;
                    }

                }
                var ret = FunctionToCall.Definition.Compile(mod, builder, param);
                //Scope.PopFunction();
                return ret;

            }

            // Regular.
            else {
                Function currFunc = Scope.PeekCurrentFunction;
                LLVMValueRef funcToCall = null;
                if (FunctionToCall.Extern || FunctionToCall.ModulePath.Equals(currFunc.ModulePath)) {
                    funcToCall = FunctionToCall.LLVMVal;
                } else {
                    if (!FunctionToCall.ModulePath.Equals(currFunc.ModulePath) && !FunctionToCall.Inline) {
                        if (!FunctionToCall.ExternedLLVMVals.ContainsKey(currFunc.ModulePath)) {
                            FunctionToCall.ExternedLLVMVals.Add(currFunc.ModulePath, mod.AddFunction(FunctionToCall.ToString(), FunctionToCall.GetFuncTypeLLVM()));
                        }
                        funcToCall = FunctionToCall.ExternedLLVMVals[currFunc.ModulePath];
                    }
                }
                return new ReturnValue(builder.BuildCall(funcToCall, args));
            }

        }

        public override string ToString() {
            return "(" + ToCall.ToString() + "(" + Parameters.ToString() + ")" + ")";
        }

    }

}