using System.Collections.Generic;
using LLVMSharp;
using LLVMSharp.Interop;

namespace StraitJacketLib.Constructs {

    // Operator that returns a value from two expressions.
    public class ExpressionOperator : Expression {
        public List<Expression> Inputs = new List<Expression>();
        public Operator Operator;
        public bool Assignment;
        VarType InputTypes = null;
        VarType RetType = null;
        bool IsImplFunction = false;
        Function FetchedOpFunction = null;

        public ExpressionOperator(List<Expression> inputs, Operator op, bool assign = false) {
            Inputs = inputs;
            Operator = op;
            Assignment = assign;

            // Compiler hack, convert variable expression to constant string if needed.
            if (op == Operator.Member && Inputs[1] as ExpressionVariable != null) { 
                Inputs[1] = new ExpressionConstStringPtr((Inputs[1] as ExpressionVariable).ToResolve.Path);
            }
            
        }

        public override void ResolveVariables() {
            foreach (var e in Inputs) {
                e.ResolveVariables();
            }
        }

        public override void ResolveTypes() {
            bool IsPrimitiveNumberSome(VarTypeEnum type) => type == VarTypeEnum.PrimitiveSimple || type == VarTypeEnum.PrimitiveInteger || type == VarTypeEnum.PrimitiveFixed;
            bool IsPrimitiveNumber(VarType t) {
                if (IsPrimitiveNumberSome(t.Type)) return true;
                if (t.Type != VarTypeEnum.PrimitiveSimple) return false;
                return false; // TODO: FLOATING POINT CHECK.
            }
            foreach (var e in Inputs) {
                e.ResolveTypes();
            }
            if (Assignment) {
                switch (Operator) {
                    case Operator.AssignEq:
                        LValue = false;
                        GenerateCastsIfNeeded2();
                        FetchedOpFunction = FetchOperatorFunction();
                        RetType = FetchedOpFunction != null ? FetchedOpFunction.ReturnType : new VarTypeSimplePrimitive(SimplePrimitives.Void);
                        return;
                    default:
                        throw new System.NotImplementedException("Operator return type not implemented!");
                }
            }
            switch (Operator) {
                case Operator.Add:
                case Operator.Sub:
                case Operator.Mul:
                case Operator.Div:
                case Operator.Mod:
                case Operator.Exp:
                    LValue = false;
                    if (IsPrimitiveNumber(Inputs[0].ReturnType()) && IsPrimitiveNumber(Inputs[1].ReturnType())) GenerateCastsIfNeeded2();
                    FetchedOpFunction = FetchOperatorFunction();
                    if (FetchedOpFunction == null) throw new System.Exception("Can't find operator overload " + Operator + " for given expression.");
                    RetType = FetchedOpFunction.ReturnType;
                    break;
                case Operator.Eq:
                case Operator.Neq:
                case Operator.Lt:
                case Operator.Gt:
                case Operator.Le:
                case Operator.Ge:
                    LValue = false;
                    GenerateCastsIfNeeded2();
                    RetType = new VarTypeSimplePrimitive(SimplePrimitives.Bool);
                    break;
                case Operator.AddressOf:
                    if (!Inputs[0].LValue) throw new System.Exception("Can't take the address of a non-lvalue!");
                    LValue = false;
                    RetType = new VarTypePointer(Inputs[0].ReturnType());
                    break;
                case Operator.Dereference:
                    if (Inputs[0].ReturnType().Type != VarTypeEnum.Pointer) {
                        throw new System.Exception("Can't take the de-reference of a non-pointer!");
                    } else {
                        RetType = (Inputs[0].ReturnType() as VarTypePointer).PointedTo;
                    }
                    break;
                case Operator.Member:
                    var eee = Inputs[0].ReturnType();
                    if (Inputs[0].ReturnType().Type == VarTypeEnum.Pointer) { // `->` does not exist in Asylum, so convert ptr.member to (*ptr).member. This will work efficiently for non-const expressions only!
                        Inputs[0] = new ExpressionOperator(new List<Expression>() { Inputs[0] }, Operator.Dereference);
                        Inputs[0].ResolveTypes();
                    }
                    if (Inputs[0].ReturnType().Type != VarTypeEnum.Tuple) {
                        throw new System.Exception("Can't take the member of an expression that doesn't result in a tuple or struct!");
                    }
                    if (Inputs[1] as ExpressionConstStringPtr == null) throw new System.Exception("Can only take the member of using a constant string pointer!");
                    RetType = (Inputs[0].ReturnType() as VarTypeStruct).GetMemberType((Inputs[1] as ExpressionConstStringPtr).Str);
                    IsImplFunction = RetType.Type == VarTypeEnum.PrimitiveFunction;
                    break;
                default:
                    throw new System.NotImplementedException("Operator return type not implemented!");
            }
        }

        // Generate casts when needed for two inputs.
        private void GenerateCastsIfNeeded2() {
            if (Inputs[0].ReturnType().Equals(Inputs[1].ReturnType())) { // Matching type.
                InputTypes = Inputs[0].ReturnType();
            } else if (Inputs[0].ReturnType().CanImplicitlyCastTo(Inputs[1].ReturnType())) { // Left can cast to right.
                ExpressionCast cast = new ExpressionCast(Inputs[0], Inputs[1].ReturnType());
                cast.ResolveTypes();
                Inputs[0] = cast;
                InputTypes = Inputs[1].ReturnType();
            } else if (Inputs[1].ReturnType().CanImplicitlyCastTo(Inputs[0].ReturnType())) { // Right can cast to left.
                ExpressionCast cast = new ExpressionCast(Inputs[1], Inputs[0].ReturnType());
                cast.ResolveTypes();
                Inputs[1] = cast;
                InputTypes = Inputs[0].ReturnType();
            } else if (false) { // Custom operator defined. TODO!!!

            } else { // Can't cast!
                throw new System.Exception("No valid casting conversion!");
            }
        }

        // Fetch an operator function.
        private Function FetchOperatorFunction() {
            Scope root = Scope.Root;
            switch (Operator) {
                case Operator.Add:
                case Operator.Sub:
                case Operator.Mul:
                case Operator.Div:
                case Operator.Mod:
                case Operator.Exp:
                    break;
            }
            return null;
        }

        public override bool IsPlural() {
            return false;
        }

        public override VarType GetReturnType() {
            return RetType;
        }

        public override void StoreSingle(ReturnValue src, ReturnValue dest, VarType srcType, VarType destType, LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            Inputs[0].StoreSingle(src, dest, srcType, destType, mod, builder, param);
        }

        public override void StorePlural(ReturnValue src, ReturnValue dest, VarType srcType, VarType destType, LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            Inputs[0].StorePlural(src, dest, srcType, destType, mod, builder, param);
        }

        public override ReturnValue Compile(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            LLVMValueRef v1, v2;
            Expression tmp;
            if (Assignment) {
                if (FetchedOpFunction == null) {
                    switch (Operator) {
                        case Operator.AssignEq:
                            tmp = new ExpressionStore(Inputs[1], Inputs[0]);
                            tmp.ResolveTypes();
                            return tmp.Compile(mod, builder, param);
                        default:
                            throw new System.NotImplementedException();
                    }
                } else {
                    throw new System.NotImplementedException();
                }
            }
            switch (Operator) {
                case Operator.Lt:
                    v1 = Inputs[0].Compile(mod, builder, param).Val;
                    if (Inputs[0].LValue) v1 = builder.BuildLoad(v1, "SJ_Load");
                    v2 = Inputs[1].Compile(mod, builder, param).Val;
                    if (Inputs[1].LValue) v2 = builder.BuildLoad(v1, "SJ_Load");
                    if (InputTypes.IsFloatingPoint()) {
                        return new ReturnValue(
                            builder.BuildFCmp(LLVMRealPredicate.LLVMRealOLT,
                            v1,
                            v2,
                            "SJ_FCompare_LT")
                        );
                    } else {
                        return new ReturnValue(
                            builder.BuildICmp(InputTypes.IsUnsigned() ? LLVMIntPredicate.LLVMIntULT : LLVMIntPredicate.LLVMIntSLT,
                            v1,
                            v2,
                            "SJ_ICompare_LT")
                        );
                    }
                case Operator.AddressOf:
                    return Inputs[0].Compile(mod, builder, param);
                case Operator.Dereference:
                    return new ReturnValue(builder.BuildLoad(
                        Inputs[0].Compile(mod, builder, param).Val,
                        "SJ_Dereference"
                    ));
                case Operator.Member:
                    v1 = Inputs[0].Compile(mod, builder, param).Val;
                    string member = (Inputs[1] as ExpressionConstStringPtr).Str;
                    if (IsImplFunction) {
                        var func = (Inputs[0].ReturnType() as VarTypeStruct).GetImplFunction(member);
                        LLVMValueRef funcToCall = null;
                        Function currFunc = Scope.PeekCurrentFunction;
                        if (func.Extern || func.ModulePath.Equals(currFunc.ModulePath)) {
                            funcToCall = func.LLVMVal;
                        } else {
                            if (!func.ModulePath.Equals(currFunc.ModulePath) && !func.Inline) {
                                if (!func.ExternedLLVMVals.ContainsKey(currFunc.ModulePath)) {
                                    func.ExternedLLVMVals.Add(currFunc.ModulePath, mod.AddFunction(func.ToString(), func.GetFuncTypeLLVM()));
                                }
                                funcToCall = func.ExternedLLVMVals[currFunc.ModulePath];
                            }
                        }
                        return new ReturnValue(funcToCall);
                    }
                    return new ReturnValue(builder.BuildStructGEP(
                        v1,
                        (Inputs[0].ReturnType() as VarTypeStruct).CalcIdx(member),
                        "SJ_Member_" + member
                    ));
            }
            throw new System.NotImplementedException("Operator has not been implemented yet!");
        }

        public override string ToString() {
            switch (Operator) {
                case Operator.Add:
                    return "(" + Inputs[0].ToString() + " + " + Inputs[1].ToString() + ")";
                case Operator.Lt:
                    return "(" + Inputs[0].ToString() + " < " + Inputs[1].ToString() + ")";
                case Operator.AddressOf:
                    return "(&" + Inputs[0].ToString() + ")";
                case Operator.Dereference:
                    return "(*" + Inputs[0].ToString() + ")";
                case Operator.Member:
                    return Inputs[0].ToString() + "." + (Inputs[1] as ExpressionConstStringPtr).Str;
            }
            return "???";
        }

    }

}