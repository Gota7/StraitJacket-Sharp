using System;
using System.Collections.Generic;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using LLVMSharp;
using WARD.Constructs;

namespace Asylum.AST {

    public partial class Visitor : IAsylumVisitor<AsylumVisitResult> {

        public AsylumVisitResult VisitUsingShortcut([NotNull] AsylumParser.UsingShortcutContext context)
        {
            Builder.Typedef(context.variable_type().Accept(this).VariableType, context.IDENTIFIER().GetText());
            return null;
        }

        // TODO: IMPLEMENTS, PROPERTIES!!!
        public AsylumVisitResult VisitStruct_definition([NotNull] AsylumParser.Struct_definitionContext context)
        {
            //List<VarTypeStr> typeImplements = context.type_implements().Accept(this).VariableTypes;
            if (context.access_modifier() != null) {
                Builder.PushModifier(context.access_modifier().Accept(this).Modifier);
            }
            Builder.BeginStruct(context.IDENTIFIER().GetText(), new VarTypeStruct[] {});
            if (context.access_modifier() != null) {
                Builder.PopModifier();
            }
            foreach (var e in context.struct_entry()) {
                e.Accept(this);
            }
            Builder.EndStruct();
            return null;
        }

        public AsylumVisitResult VisitStructAccess([NotNull] AsylumParser.StructAccessContext context)
        {
            throw new System.NotImplementedException();
        }

        public AsylumVisitResult VisitStructData([NotNull] AsylumParser.StructDataContext context)
        {
            VarParameter param = context.variable_parameter().Accept(this).Parameter;
            Builder.StructEntry(param.Value.Type, param.Value.Name);
            return null;
        }

        public AsylumVisitResult VisitVarTypePrimitive([NotNull] AsylumParser.VarTypePrimitiveContext context)
        {
            return context.primitives().Accept(this);
        }

        public AsylumVisitResult VisitPrimitiveFunction([NotNull] AsylumParser.PrimitiveFunctionContext context)
        {

            // Setup function type.
            int numParams = context.variable_type().Length;
            VarType ret = null;
            List<VarType> members = new List<VarType>();

            // Get the return type.
            if (numParams == 0) {
                ret = new VarTypeSimplePrimitive(SimplePrimitives.Void);
            } else {
                ret = context.variable_type()[0].Accept(this).VariableType;
            }

            // Get parameters if needed.
            if (numParams > 1) {
                for (int i = 0; i < numParams - 1; i++) {
                    members.Add(context.variable_type()[i + 1].Accept(this).VariableType);
                }
            }

            // Finish.
            return new AsylumVisitResult() { VariableType = new VarTypeFunction(ret, members) };

        }

        public AsylumVisitResult VisitPrimitiveBool([NotNull] AsylumParser.PrimitiveBoolContext context)
        {
            return new AsylumVisitResult() {
                VariableType = new VarTypeSimplePrimitive(SimplePrimitives.Bool)
            };
        }

        public AsylumVisitResult VisitPrimitiveChar([NotNull] AsylumParser.PrimitiveCharContext context)
        {
            return new AsylumVisitResult() {
                VariableType = new VarTypeSimplePrimitive(SimplePrimitives.Char)
            };
        }

        public AsylumVisitResult VisitPrimitiveString([NotNull] AsylumParser.PrimitiveStringContext context)
        {
            // TEMPORARY!!!
            return new AsylumVisitResult() {
                VariableType = new VarTypeSimplePrimitive(SimplePrimitives.ConstString)
            };
        }

        public AsylumVisitResult VisitPrimitiveUnsigned([NotNull] AsylumParser.PrimitiveUnsignedContext context)
        {
            return new AsylumVisitResult() {
                VariableType = new VarTypeInteger(false, uint.Parse(context.UNSIGNED().GetText().Substring(1)))
            };
        }

        public AsylumVisitResult VisitPrimitiveSigned([NotNull] AsylumParser.PrimitiveSignedContext context)
        {
            return new AsylumVisitResult() {
                VariableType = new VarTypeInteger(true, uint.Parse(context.SIGNED().GetText().Substring(1)))
            };
        }

        public AsylumVisitResult VisitPrimitiveHalf([NotNull] AsylumParser.PrimitiveHalfContext context)
        {
            return new AsylumVisitResult() {
                VariableType = new VarTypeFloating(16)
            };
        }

        public AsylumVisitResult VisitPrimitiveFloat([NotNull] AsylumParser.PrimitiveFloatContext context)
        {
            return new AsylumVisitResult() {
                VariableType = new VarTypeFloating(32)
            };
        }

        public AsylumVisitResult VisitPrimitiveDouble([NotNull] AsylumParser.PrimitiveDoubleContext context)
        {
            return new AsylumVisitResult() {
                VariableType = new VarTypeFloating(64)
            };
        }

        public AsylumVisitResult VisitPrimitiveExtended([NotNull] AsylumParser.PrimitiveExtendedContext context)
        {
           return new AsylumVisitResult() {
                VariableType = new VarTypeFloating(80)
            };
        }

        public AsylumVisitResult VisitPrimitiveDecimal([NotNull] AsylumParser.PrimitiveDecimalContext context)
        {
            return new AsylumVisitResult() {
                VariableType = new VarTypeFloating(128)
            };
        }

        public AsylumVisitResult VisitPrimitiveFixed([NotNull] AsylumParser.PrimitiveFixedContext context)
        {
            string[] split = context.FIXED().GetText().Replace("fix", "").Split('x');
            return new AsylumVisitResult() {
                VariableType = new VarTypeFixed(uint.Parse(split[0]), uint.Parse(split[1]))
            };
        }

        public AsylumVisitResult VisitPrimitiveObject([NotNull] AsylumParser.PrimitiveObjectContext context)
        {
            return new AsylumVisitResult() {
                VariableType = new VarTypeSimplePrimitive(SimplePrimitives.Object)
            };
        }

        public AsylumVisitResult VisitVarTypeArray([NotNull] AsylumParser.VarTypeArrayContext context)
        {
            List<uint> lens = new List<uint>();
            int numItems = context.GetText().Split(',').Length;
            for (int i = 0 ; i < numItems; i++) lens.Add(0);
            if (context.INTEGER().Length != 0 && numItems != context.INTEGER().Length) throw new Exception("The size of a multi-dimensional array either has to be constant or nothing!");
            for (int i = 0; i < context.INTEGER().Length; i++) {
                lens[i] = (uint)GetInteger(context.INTEGER()[i]).ValueWhole;
            }
            return new AsylumVisitResult() {
                VariableType = new VarTypeArray(
                    context.variable_type().Accept(this).VariableType,
                    lens
                )
            };
        }

        public AsylumVisitResult VisitVarTypePointer([NotNull] AsylumParser.VarTypePointerContext context)
        {
            return new AsylumVisitResult() {
                VariableType = new VarTypePointer(context.variable_type().Accept(this).VariableType)
            };
        }

        public AsylumVisitResult VisitVarTypeReference([NotNull] AsylumParser.VarTypeReferenceContext context)
        {
            return new AsylumVisitResult() {
                VariableType = new VarTypeReference(context.variable_type().Accept(this).VariableType)
            };
        }

        public AsylumVisitResult VisitVarTypeCustom([NotNull] AsylumParser.VarTypeCustomContext context)
        {
            return new AsylumVisitResult() {
                VariableType = new VarTypeCustom(context.variable_or_function().Accept(this).VariableOrFunction)
            };
        }

        public AsylumVisitResult VisitVarTypeThis([NotNull] AsylumParser.VarTypeThisContext context)
        {
            return new AsylumVisitResult() {
                VariableType = Builder.ThisType()
            };
        }

        public AsylumVisitResult VisitVarTypeConstant([NotNull] AsylumParser.VarTypeConstantContext context)
        {
            var ret = context.variable_type().Accept(this).VariableType;
            ret.Constant = true;
            return new AsylumVisitResult() { VariableType = ret };
        }

        public AsylumVisitResult VisitVarTypeGenerics([NotNull] AsylumParser.VarTypeGenericsContext context)
        {
            List<VarType> typeImplements = new List<VarType>();
            for (int i = 1; i < context.variable_type().Length; i++) {
                typeImplements.Add(context.variable_type()[i].Accept(this).VariableType);
            }
            return new AsylumVisitResult() { VariableType = new VarTypeGenericInstance(
                context.variable_type()[0].Accept(this).VariableType,
                typeImplements
            )};
        }

    }
    
}