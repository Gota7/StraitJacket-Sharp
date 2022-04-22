using System.Collections.Generic;
using System.Linq;
using LLVMSharp;
using LLVMSharp.Interop;

/*

    Expressions:
        So there are many different types of expressions that can be created.
        The purpose of this file is to provide generic methods for them.

*/

namespace WARD.Constructs {

    // Variable or function that should be resolved.
    public class VariableOrFunction {
        public string Path;
        public Scope Scope;

        public List<Variable> ResolveVariable() {
            return Scope.ResolveVariable(this);
        }

        public Function ResolveFunctionVariable(List<VarType> paramTypes, VarType expectedReturnType = null) {
            return Scope.ResolveFunctionVariable(this, paramTypes, expectedReturnType);
        }

        public override string ToString() {
            return Path;
        }

    }

    // Value types.
    public enum ExpressionType {
        ArrayAllocate,
        Call,
        Cast,
        Comma,
        ConstArrayAllocate,
        ConstFixed,
        ConstFloat,
        ConstInt,
        ConstSimple,
        ConstString,
        Operator,
        Store,
        Variable
    }

    // Operator.
    public enum Operator {
        Add,
        Sub,
        Mul,
        Div,
        Mod,
        Exp,
        Range,
        RangeEq,
        BitAnd,
        BitOr,
        BitXor,
        BitNot,
        LShift,
        RShift,
        Pos,
        Neg,
        PreInc,
        PreDec,
        Inc,
        Dec,
        Member,
        Dereference,
        AddressOf,
        AsAddress,
        And,
        Or,
        Nand,
        Nor,
        Not,
        Eq,
        Neq,
        Gt,
        Lt,
        Ge,
        Le,
        Cmp,
        Cond,
        Null,
        AssignEq,
        Is,
        As,
        Lambda,
        Ternary,
        ArrayAccess,
        FromLast
    }

    // Context for resolution.
    public class TypeResolutionContext {
        public VarType PreferredType; // What the expression is in total.
        public List<VarType> OtherRelations = new List<VarType>(); // What the other operand's types is to choose.

        // Given a list of possible types to choose from, choose the one that matches. May return null.
        public VarType ChooseType(List<VarType> options) {

            // Match count.
            int matchCount = 0;
            VarType lastMatch = null;

            // Try for exact match.
            foreach (var t in OtherRelations) {
                foreach (var o in options) {
                    if (t.Equals(o)) {
                        matchCount++;
                        lastMatch = t;
                    }
                }
            }

            // Check for ambiguity.
            if (matchCount > 1) return null;
            else if (matchCount == 1) return lastMatch;

            // Ok, no takers, try to see if casting is an option.
            foreach (var t in OtherRelations) {
                foreach (var o in options) {
                    if (o.CanImplicitlyCastTo(t)) {
                        matchCount++;
                        lastMatch = t;
                    }
                }
            }

            // Check for ambiguity.
            if (matchCount > 1) return null;
            else if (matchCount == 1) return lastMatch;

            // No types matched, so go by the preferred type.
            foreach (var o in options) {
                if (o.CanImplicitlyCastTo(PreferredType)) {
                    return o;
                }
            }

            // Don't know what to choose.
            return null;

        }

    }

    // For resolving expressions.
    public class ExpressionResolutionContext {
        public Expression Parent; // Parent expression.
        public List<Expression> Neighbors = new List<Expression>(); // Expressions next to the current one.
        public VarType PreferredType; // Default desired type by the parent expression.
        
    }

    // Expression. TODO: Improve how context is shared throughout expressions.
    public abstract class Expression : ICompileable {
        public ExpressionType Type;
        public bool LValue = true;
        public ExpressionResolutionContext Ctx = new ExpressionResolutionContext();
        public FileContext FileContext;

        public FileContext GetFileContext() => FileContext;

        // Vfunctions.
        public virtual void ResolveVariables() {} // Resolve variable and function call references to a list of possibilities.
        public virtual void ResolveTypes(VarType preferredReturnType, List<VarType> parameterTypes) {} // Resolve types, type check, add casts, and solidify all function references. The parameters are so calls can have expressions resolve the correct function.
        public abstract VarType GetReturnType(); // Get the return type of an expression.
        public abstract bool IsPlural(); // If this expression type returns or stores multiple values.
        public abstract void StoreSingle(ReturnValue src, ReturnValue dest, VarType srcType, VarType destType, LLVMModuleRef mod, LLVMBuilderRef builder, object param); // Store a single value into the expression.
        public abstract void StorePlural(ReturnValue src, ReturnValue dest, VarType srcType, VarType destType, LLVMModuleRef mod, LLVMBuilderRef builder, object param); // Store a plural value into the expression.
        public abstract ReturnValue Compile(LLVMModuleRef mod, LLVMBuilderRef builder, object param); // Compile the expression.
        public void CompileDeclarations(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {}

        // Resolve types.
        public void ResolveTypes() => ResolveTypes(null, null);

        // Get the return type. Use this instead of GetReturnType.
        public VarType ReturnType() => GetReturnType().TrueType();

    }

}