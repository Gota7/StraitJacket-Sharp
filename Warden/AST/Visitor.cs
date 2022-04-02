using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using WARD.Builder;

namespace Warden.AST {

    public partial class Visitor : ICVisitor<Result>
    {
        public ProgramBuilder Builder = new ProgramBuilder();

        public Result Visit(IParseTree tree)
        {
            throw new NotImplementedException();
        }

        public Result VisitAbstractDeclarator([NotNull] CParser.AbstractDeclaratorContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitAdditiveExpression([NotNull] CParser.AdditiveExpressionContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitAlignmentSpecifier([NotNull] CParser.AlignmentSpecifierContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitAndExpression([NotNull] CParser.AndExpressionContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitArgumentExpressionList([NotNull] CParser.ArgumentExpressionListContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitAssignmentExpression([NotNull] CParser.AssignmentExpressionContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitAssignmentOperator([NotNull] CParser.AssignmentOperatorContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitAtomicTypeSpecifier([NotNull] CParser.AtomicTypeSpecifierContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitCastExpression([NotNull] CParser.CastExpressionContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitChildren(IRuleNode node)
        {
            throw new NotImplementedException();
        }

        public Result VisitCompoundStatement([NotNull] CParser.CompoundStatementContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitConditionalExpression([NotNull] CParser.ConditionalExpressionContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitConstantExpression([NotNull] CParser.ConstantExpressionContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitDeclarationList([NotNull] CParser.DeclarationListContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitDeclarationSpecifier([NotNull] CParser.DeclarationSpecifierContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitDeclarationSpecifiers([NotNull] CParser.DeclarationSpecifiersContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitDeclarationSpecifiers2([NotNull] CParser.DeclarationSpecifiers2Context context)
        {
            throw new NotImplementedException();
        }

        public Result VisitDeclarator([NotNull] CParser.DeclaratorContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitDesignation([NotNull] CParser.DesignationContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitDesignator([NotNull] CParser.DesignatorContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitDesignatorList([NotNull] CParser.DesignatorListContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitDirectAbstractDeclarator([NotNull] CParser.DirectAbstractDeclaratorContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitDirectDeclarator([NotNull] CParser.DirectDeclaratorContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitEnumerationConstant([NotNull] CParser.EnumerationConstantContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitEnumerator([NotNull] CParser.EnumeratorContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitEnumeratorList([NotNull] CParser.EnumeratorListContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitEnumSpecifier([NotNull] CParser.EnumSpecifierContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitEqualityExpression([NotNull] CParser.EqualityExpressionContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitErrorNode(IErrorNode node)
        {
            throw new NotImplementedException();
        }

        public Result VisitExclusiveOrExpression([NotNull] CParser.ExclusiveOrExpressionContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitForCondition([NotNull] CParser.ForConditionContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitForDeclaration([NotNull] CParser.ForDeclarationContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitForExpression([NotNull] CParser.ForExpressionContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitFunctionSpecifier([NotNull] CParser.FunctionSpecifierContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitGccAttribute([NotNull] CParser.GccAttributeContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitGccAttributeList([NotNull] CParser.GccAttributeListContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitGccAttributeSpecifier([NotNull] CParser.GccAttributeSpecifierContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitGccDeclaratorExtension([NotNull] CParser.GccDeclaratorExtensionContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitGenericAssociation([NotNull] CParser.GenericAssociationContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitGenericAssocList([NotNull] CParser.GenericAssocListContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitGenericSelection([NotNull] CParser.GenericSelectionContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitIdentifierList([NotNull] CParser.IdentifierListContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitInclusiveOrExpression([NotNull] CParser.InclusiveOrExpressionContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitInitDeclarator([NotNull] CParser.InitDeclaratorContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitInitDeclaratorList([NotNull] CParser.InitDeclaratorListContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitInitializer([NotNull] CParser.InitializerContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitInitializerList([NotNull] CParser.InitializerListContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitIterationStatement([NotNull] CParser.IterationStatementContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitJumpStatement([NotNull] CParser.JumpStatementContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitLabeledStatement([NotNull] CParser.LabeledStatementContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitLogicalAndExpression([NotNull] CParser.LogicalAndExpressionContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitLogicalOrExpression([NotNull] CParser.LogicalOrExpressionContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitMultiplicativeExpression([NotNull] CParser.MultiplicativeExpressionContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitNestedParenthesesBlock([NotNull] CParser.NestedParenthesesBlockContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitParameterDeclaration([NotNull] CParser.ParameterDeclarationContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitParameterList([NotNull] CParser.ParameterListContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitParameterTypeList([NotNull] CParser.ParameterTypeListContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitPointer([NotNull] CParser.PointerContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitPostfixExpression([NotNull] CParser.PostfixExpressionContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitPrimaryExpression([NotNull] CParser.PrimaryExpressionContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitRelationalExpression([NotNull] CParser.RelationalExpressionContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitSelectionStatement([NotNull] CParser.SelectionStatementContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitShiftExpression([NotNull] CParser.ShiftExpressionContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitSpecifierQualifierList([NotNull] CParser.SpecifierQualifierListContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitStaticAssertDeclaration([NotNull] CParser.StaticAssertDeclarationContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitStorageClassSpecifier([NotNull] CParser.StorageClassSpecifierContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitStructDeclaration([NotNull] CParser.StructDeclarationContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitStructDeclarationList([NotNull] CParser.StructDeclarationListContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitStructDeclarator([NotNull] CParser.StructDeclaratorContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitStructDeclaratorList([NotNull] CParser.StructDeclaratorListContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitStructOrUnion([NotNull] CParser.StructOrUnionContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitStructOrUnionSpecifier([NotNull] CParser.StructOrUnionSpecifierContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitTerminal(ITerminalNode node)
        {
            throw new NotImplementedException();
        }

        public Result VisitTypedefName([NotNull] CParser.TypedefNameContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitTypeName([NotNull] CParser.TypeNameContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitTypeQualifier([NotNull] CParser.TypeQualifierContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitTypeQualifierList([NotNull] CParser.TypeQualifierListContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitTypeSpecifier([NotNull] CParser.TypeSpecifierContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitUnaryExpression([NotNull] CParser.UnaryExpressionContext context)
        {
            throw new NotImplementedException();
        }

        public Result VisitUnaryOperator([NotNull] CParser.UnaryOperatorContext context)
        {
            throw new NotImplementedException();
        }

    }

}