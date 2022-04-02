using Antlr4.Runtime.Misc;

namespace Warden.AST {

    // Misc. parse items.
    public partial class Visitor : ICVisitor<Result> {

        // A function or variable declaration.
        public Result VisitDeclaration([NotNull] CParser.DeclarationContext context) {
            throw new NotImplementedException();
        }

    }

}