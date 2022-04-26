using System.Collections.Generic;
using System.Linq;
using LLVMSharp;
using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Comma operator, which separates and returns multiple expressions.
    public class ExpressionComma : Expression {
        public List<Expression> Expressions;
        private VarType RetType;

        // Construct a comma expression given a list of expressions, and automatically split commas.
        public ExpressionComma(List<Expression> expressions) {
            Type = ExpressionType.Comma;
            Expressions = new List<Expression>();
            foreach (var e in expressions) {
                if (e.Type == ExpressionType.Comma) {
                    Expressions.AddRange((e as ExpressionComma).Expressions);
                } else {
                    Expressions.Add(e);
                }
            }
        }

        // Just resolve variables for internal expressions.
        public override void ResolveVariables() {
            foreach (var e in Expressions) {
                e.ResolveVariables();
            }
        }

        // Resolve types for internal expressions, and figure out the return type.
        public override void ResolveTypes(VarType preferredReturnType, List<VarType> parameterTypes) {
            List<VarType> memberTypes = new List<VarType>();
            foreach (var e in Expressions) {
                e.ResolveTypes(preferredReturnType, parameterTypes);
                memberTypes.Add(e.ReturnType());
            }
            RetType = new VarTypeTuple(memberTypes);
        }

        // Get the return type.
        public override VarType GetReturnType() {
            return RetType;
        }

        // Compile the internal expressions, and add them to the return value.
        public override LLVMValueRef Compile(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            List<LLVMValueRef> rets = new List<LLVMValueRef>();
            foreach (var e in Expressions) {
                var val = e.CompileRValue(mod, builder, param);
                rets.Add(val);
            }
            return LLVMValueRef.CreateConstStruct(rets.ToArray(), true);
        }

        public override string ToString() {
            string ret = "(";
            foreach (var e in Expressions) {
                ret += e.ToString();
                if (e != Expressions.Last()) {
                    ret += ", ";
                }
            }
            return ret + ")";
        }

    }

}