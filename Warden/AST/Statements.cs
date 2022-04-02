using Antlr4.Runtime.Misc;

namespace Warden.AST {

    // General statements to parse.
    public partial class Visitor : ICVisitor<Result> {

        // Contains multiple block items.
        public Result VisitBlockItemList([NotNull] CParser.BlockItemListContext context) {
            foreach (var b in context.blockItem()) {
                b.Accept(this);
            }
            return null;
        }

        // View a statement or an expression.
        public Result VisitBlockItem([NotNull] CParser.BlockItemContext context) {
            if (context.statement() != null) {
                return context.statement().Accept(this);
            } else {
                return context.declaration().Accept(this);
            }
        }

        // General statement visit.
        public Result VisitStatement([NotNull] CParser.StatementContext context) {
            if (context.labeledStatement() != null) {
                return context.labeledStatement().Accept(this);
            } else if (context.compoundStatement() != null) {
                return context.compoundStatement().Accept(this);
            } else if (context.expressionStatement() != null) {
                return context.expressionStatement().Accept(this);
            } else if (context.selectionStatement() != null) {
                return context.selectionStatement().Accept(this);
            } else if (context.iterationStatement() != null) {
                return context.iterationStatement().Accept(this);
            } else if (context.jumpStatement() != null) {
                return context.jumpStatement().Accept(this);
            } else {
                throw new System.NotImplementedException(); // Some ASM thing.
            }
        }

        // Expression statement.
        public Result VisitExpressionStatement([NotNull] CParser.ExpressionStatementContext context) {
            Result ret = new Result();
            if (context.expression() != null) ret.CodeStatement = context.expression().Accept(this).Expression;
            return ret;
        }

    }

}