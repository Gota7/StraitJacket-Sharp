using Antlr4.Runtime.Misc;
using WARD.Constructs;

namespace Warden.AST {

    // Parse expressions.
    public partial class Visitor : ICVisitor<Result> {

        // Comma operator only uses last expression.
        public Result VisitExpression([NotNull] CParser.ExpressionContext context) {
            Result ret = null;
            foreach (var e in context.assignmentExpression()) {
                Result tmp = e.Accept(this);
                if (e == context.assignmentExpression().Last()) ret = tmp;
            }
            return ret;
        }

    }

}