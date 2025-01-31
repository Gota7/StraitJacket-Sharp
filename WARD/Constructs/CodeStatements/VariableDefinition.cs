using System.Collections.Generic;
using System.Linq;
using LLVMSharp.Interop;

namespace WARD.Constructs {
    
    // Variable definition. TODO: GET DEFAULT INITIALIZER EXPRESSION!!!
    public class VariableDefinition : ICompileable {
        public List<Variable> Variables = new List<Variable>();
        public Expression Definition;
        public FileContext FileContext;
        List<Expression> GeneratedExpressions = new List<Expression>();

        public FileContext GetFileContext() => FileContext;

        public VariableDefinition(List<Variable> variables, Expression definition) {

            // Split into individual expressions.
            Variables = variables;
            Definition = definition;
            if (Definition != null) {
                foreach (var v in variables) {
                    GeneratedExpressions.Add(
                        new ExpressionStore(Definition, new ExpressionVariable(
                            new VariableOrFunction() { Scope = v.Scope, Path = v.Name })));
                }
            } else {
                foreach (var v in variables) {
                    GeneratedExpressions.Add(
                        new ExpressionStore(v.Type.DefaultValue(), new ExpressionVariable(
                            new VariableOrFunction() { Scope = v.Scope, Path = v.Name })));
                }
            }

        }

        public void ResolveVariables() {
            var h = ToString();
            if (Definition != null) Definition.ResolveVariables();
            foreach (var e in GeneratedExpressions) {
                e.ResolveVariables();
            }
        }

        public void ResolveTypes() {
            if (Definition != null) Definition.ResolveTypes();
            foreach (var e in GeneratedExpressions) {
                e.ResolveTypes();
            }
        }

        public void CompileDeclarations(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            foreach (var v in Variables) {
                v.LLVMValue = builder.BuildAlloca(v.Type.GetLLVMType(), "SJ_Define_" + v.Name);
            }
        }

        public ReturnValue Compile(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {

            // Only compile if not dead.
            if (CodeStatements.BlockTerminated) return null;

            // Compile store expressions.
            foreach (var e in GeneratedExpressions) {
                e.Compile(mod, builder, param);
            }
            return new ReturnValue();
            
        }

        public override string ToString() {
            string ret = "";
            foreach (var v in Variables) {
                ret += v.Type.ToString() + " " + v.Name;
                if (v != Variables.Last()) {
                    ret += ", ";
                }
            }
            if (Definition != null) ret += " = " + Definition.ToString();
            return ret;
        }

    }

}