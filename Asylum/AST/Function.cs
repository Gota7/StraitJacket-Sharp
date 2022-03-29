using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using WARD.Constructs;

// There is a lot that still needs to be done here, and __thiscall is not even implemented.
namespace Asylum.AST {

    public partial class Visitor : IAsylumVisitor<AsylumVisitResult> {
    
        // TODO!!!
        public AsylumVisitResult VisitExtern_function_definition([NotNull] AsylumParser.Extern_function_definitionContext context)
        {

            // Get attributes. TODO!!!
            foreach (var a in context.attribute()) {

            }

            // Get modifiers.
            Modifier modifier = Modifier.None;
            foreach (var p in context.extern_function_property()) {
                var res = p.Accept(this);
                if (res == null) continue;
                modifier |= res.Modifier;
            }

            // Get parameters.
            List<VarParameter> parameters;
            if (context.variable_arguments() != null) {
                parameters = context.variable_arguments().Accept(this).Parameters;
            } else {
                parameters = new List<VarParameter>();
            }
    
            // Get return type.
            VarType returnType;
            if (context.variable_type() != null) {
                returnType = context.variable_type().Accept(this).VariableType;
            } else {
                returnType = new VarTypeSimplePrimitive(SimplePrimitives.Void);
            }

            // Finished.
            Builder.PushModifier(modifier);
            Builder.ExternFunction(context.IDENTIFIER().GetText(), returnType, parameters);
            Builder.PopModifier();
            return null;

        }

        public AsylumVisitResult VisitExtern_function_property([NotNull] AsylumParser.Extern_function_propertyContext context)
        {
            if (context.ASYNC() != null) {
                return new AsylumVisitResult() { Modifier = Modifier.Async };
            } else if (context.STATIC() != null) {
                return new AsylumVisitResult() { Modifier = Modifier.Static };
            }
            return null;
        }

        public AsylumVisitResult VisitAttribute([NotNull] AsylumParser.AttributeContext context)
        {
            throw new System.NotImplementedException();
        }

        // TODO!!!
        public AsylumVisitResult VisitFunction_definition([NotNull] AsylumParser.Function_definitionContext context)
        {

            // Get attributes. TODO!!!
            foreach (var a in context.attribute()) {

            }

            // GET MODIFIERS TODO!!!
            Modifier modifier = Modifier.None;
            foreach (var p in context.function_property()) {
                if (p.INLINE() != null) modifier |= Modifier.Inline;
            }

            // GENERICS TODO!!!

            // Get parameters.
            List<VarParameter> parameters;
            if (context.variable_arguments() != null) {
                parameters = context.variable_arguments().Accept(this).Parameters;
            } else {
                parameters = new List<VarParameter>();
            }
    
            // Get return type.
            VarType returnType;
            if (context.variable_type() != null) {
                returnType = context.variable_type().Accept(this).VariableType;
            } else {
                returnType = new VarTypeSimplePrimitive(SimplePrimitives.Void);
            }

            // Get code.
            Builder.PushModifier(modifier);
            if (ImplFunc) {
                Builder.BeginImplFunction(context.IDENTIFIER().GetText(), returnType, parameters);
                ImplFunc = false;
            } else {
                Builder.BeginFunction(context.IDENTIFIER().GetText(), returnType, parameters);
            }
            Builder.PopModifier();
            if (context.expression() != null) {
                if (returnType.Equals(new VarTypeSimplePrimitive(SimplePrimitives.Void))) { // Hack for accidentally returning a value instead of void which is illegal.
                    Builder.Code(context.expression().Accept(this).Expression);
                } else {
                    Builder.Code(new ReturnStatement(context.expression().Accept(this).Expression));
                }
            } else if (context.code_statement() != null) {
                foreach (var c in context.code_statement()) {
                    c.Accept(this); // Add code statements.
                }
            }

            // Finish.
            Builder.EndFunction();
            return null;

        }

        // TODO!!!
        public AsylumVisitResult VisitConstructor_definition([NotNull] AsylumParser.Constructor_definitionContext context)
        {
            
            // Get attributes. TODO!!!
            foreach (var a in context.attribute()) {

            }

            // GET MODIFIERS TODO!!!
            Modifier modifier = Modifier.None;
            foreach (var p in context.function_property()) {
                if (p.INLINE() != null) modifier |= Modifier.Inline;
            }

            // GENERICS TODO!!!

            // Get parameters.
            List<VarParameter> parameters;
            if (context.variable_arguments() != null) {
                parameters = context.variable_arguments().Accept(this).Parameters;
            } else {
                parameters = new List<VarParameter>();
            }

            // Get code.
            Builder.PushModifier(modifier);
            Builder.BeginImplConstructor(parameters);
            Builder.PopModifier();
            if (context.expression() != null) {
                Builder.Code(new ReturnStatement(context.expression().Accept(this).Expression));
            } else if (context.code_statement() != null) {
                foreach (var c in context.code_statement()) {
                    c.Accept(this); // Add code statements.
                }
            }

            // Finish.
            Builder.EndFunction();
            return null;

        }

        // TODO!!!
        public AsylumVisitResult VisitOperator_definition([NotNull] AsylumParser.Operator_definitionContext context)
        {

            // Get attributes. TODO!!!
            foreach (var a in context.attribute()) {

            }

            // GET MODIFIERS TODO!!!
            Modifier modifier = Modifier.None;
            if (context.INLINE() != null) modifier |= Modifier.Inline;

            // GENERICS TODO!!!

            // Get parameters.
            List<VarParameter> parameters;
            if (context.variable_arguments() != null) {
                parameters = context.variable_arguments().Accept(this).Parameters;
            } else {
                parameters = new List<VarParameter>();
            }
    
            // Get return type.
            VarType returnType;
            if (context.variable_type() != null) {
                returnType = context.variable_type().Accept(this).VariableType;
            } else {
                returnType = new VarTypeSimplePrimitive(SimplePrimitives.Void);
            }

            // Get code.
            Builder.PushModifier(modifier);
            var opRes = context.@operator().Accept(this);
            Builder.BeginImplOperator(opRes.Operator, opRes.OperatorEqFlag, returnType, parameters);
            Builder.PopModifier();
            if (context.expression() != null) {
                if (returnType.Equals(new VarTypeSimplePrimitive(SimplePrimitives.Void))) { // Hack for accidentally returning a value instead of void which is illegal.
                    Builder.Code(context.expression().Accept(this).Expression);
                } else {
                    Builder.Code(new ReturnStatement(context.expression().Accept(this).Expression));
                }
            } else if (context.code_statement() != null) {
                foreach (var c in context.code_statement()) {
                    c.Accept(this); // Add code statements.
                }
            }

            // Finish.
            Builder.EndFunction();
            return null;

        }
    
    }

}