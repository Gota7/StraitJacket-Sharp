using System;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using LLVMSharp;
using WARD.Builder;
using WARD.Constructs;

namespace Asylum.AST {

    public partial class Visitor : IAsylumVisitor<AsylumVisitResult> {
        public ProgramBuilder Builder = new ProgramBuilder();

        public AsylumVisitResult VisitInit([NotNull] AsylumParser.InitContext context)
        {
            for (int i = 0; i < context.universal_statement().Length; i++) {
                context.universal_statement()[i].Accept(this);
            }
            return null;
        }

        public AsylumVisitResult VisitUniversalExternFunction([NotNull] AsylumParser.UniversalExternFunctionContext context)
        {
            var ret = context.extern_function_definition().Accept(this);
            return ret;
        }

        public AsylumVisitResult VisitUniversalFunction([NotNull] AsylumParser.UniversalFunctionContext context)
        {
            var ret = context.function_definition().Accept(this);
            return ret;
        }

        public AsylumVisitResult VisitUniversalUsing([NotNull] AsylumParser.UniversalUsingContext context)
        {
            return context.using_statement().Accept(this);
        }

        public AsylumVisitResult VisitUniversalStruct([NotNull] AsylumParser.UniversalStructContext context)
        {
            return context.struct_definition().Accept(this);
        }

        public AsylumVisitResult VisitUniversalImplementation([NotNull] AsylumParser.UniversalImplementationContext context)
        {
            return context.implementation_definition().Accept(this);
        }

        public AsylumVisitResult VisitUniversalTopLevelCode([NotNull] AsylumParser.UniversalTopLevelCodeContext context)
        {
            context.code_statement().Accept(this);
            return null;
        }

    }

}