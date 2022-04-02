using Antlr4.Runtime.Misc;
using WARD.Constructs;

namespace Warden.AST {

    // Top level statements to parse.
    public partial class Visitor : ICVisitor<Result> {

        // Main entrypoint.
        public Result VisitCompilationUnit([NotNull] CParser.CompilationUnitContext context) {
            if (context.translationUnit() != null) context.translationUnit().Accept(this);
            return null;
        }

        // Contains at least one external declaration.
        public Result VisitTranslationUnit([NotNull] CParser.TranslationUnitContext context) {
            foreach (var e in context.externalDeclaration()) {
                e.Accept(this);
            }
            return null;
        }

        // This is either a declaration, or a function definition.
        public Result VisitExternalDeclaration([NotNull] CParser.ExternalDeclarationContext context)
        {
            if (context.declaration() != null) {
                context.declaration().Accept(this);
            } else if (context.functionDefinition() != null) {
                context.functionDefinition().Accept(this);
            }
            return null;
        }

        // Define a function.
        public Result VisitFunctionDefinition([NotNull] CParser.FunctionDefinitionContext context) {

            // Get the destination type, int by default.
            VarType retType = new VarTypeInteger(true, 32);
            if (context.declarationSpecifiers() != null) {
                retType = context.declarationSpecifiers().Accept(this).Type;
            }

            // TODO: DECLARATOR!!!

            // TODO: DECLARATION LIST!!!

            // Get code statements after defining function.
            Builder.BeginFunction("TODO", retType, null); // TODO!!!
            context.compoundStatement().Accept(this);
            Builder.EndFunction();
            throw new System.NotImplementedException();

        }

    }

}