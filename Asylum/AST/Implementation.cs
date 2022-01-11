using System;
using System.Collections.Generic;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using LLVMSharp;
using StraitJacketLib.Constructs;

namespace Asylum.AST {

    public partial class Visitor : IAsylumVisitor<AsylumVisitResult> {
        bool ImplFunc = false;

        public AsylumVisitResult VisitImplementation_definition([NotNull] AsylumParser.Implementation_definitionContext context)
        {
            VarType implements = null;
            VarType baseType = context.variable_type()[0].Accept(this).VariableType;
            if (context.variable_type().Length > 1) {
                implements = baseType;
                baseType = context.variable_type()[1].Accept(this).VariableType;
            }
            Builder.BeginImplementation(
                baseType,
                implements
            );
            foreach (var e in context.implementation_entry()) {
                e.Accept(this);
            }
            Builder.EndImplementation();
            return null;
        }

        public AsylumVisitResult VisitImplementationEntryCast([NotNull] AsylumParser.ImplementationEntryCastContext context)
        {
            throw new System.NotImplementedException();
            /*var ret = context.cast_definition().Accept(this).Function;
            //CTX.Implementation.cast.Add(ret.Name, ret); // TODO: PROPER NAME MANGLING!!!
            return null;*/
        }

        public AsylumVisitResult VisitImplementationEntryConstructor([NotNull] AsylumParser.ImplementationEntryConstructorContext context)
        {
            throw new System.NotImplementedException();
            /*var ret = context.constructor_definition().Accept(this).Function;
            CTX.Implementation.Functions.Add(ret.ToString(), ret);
            return null;*/
        }

        public AsylumVisitResult VisitImplementationEntryFunction([NotNull] AsylumParser.ImplementationEntryFunctionContext context)
        {
            ImplFunc = true;
            context.function_definition().Accept(this);
            return null;
        }

        public AsylumVisitResult VisitImplementationEntryOperator([NotNull] AsylumParser.ImplementationEntryOperatorContext context)
        {
            throw new System.NotImplementedException();
            /*var ret = context.operator_definition().Accept(this).Function;
            CTX.Implementation.Operators.Add(ret.Operator, ret);
            return null;*/
        }

    }

}