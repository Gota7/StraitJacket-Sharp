namespace WARD.Constructs {

    // Binary comparison operator with integers or pointers.
    public abstract class BinaryComparison : OperatorBase {
        protected bool Floating;
        protected bool Signed;

        // Create a new math operator expression.
        public BinaryComparison(Expression left, Expression right, string opName) : base(opName, left, right) {}

        protected override void ResolveTypesDefault() {

            // Make sure types are numeric or pointers.
            VarType leftType = Args[0].ReturnType();
            if (!leftType.IsNumeric()) {
                if (leftType.Type == VarTypeEnum.Pointer) {
                    leftType = VarType.TypePtrAsInt;
                    Args[0] = new ExpressionCast(Args[0], VarType.TypePtrAsInt);
                    Args[0].ResolveTypes();
                } else {
                    throw new System.Exception("Left argument of comparison operator must be a number type or pointer!");
                }
            }
            VarType rightType = Args[1].ReturnType();
            if (!rightType.IsNumeric()) {
                if (rightType.Type == VarTypeEnum.Pointer) {
                    rightType = VarType.TypePtrAsInt;
                    Args[1] = new ExpressionCast(Args[1], VarType.TypePtrAsInt);
                    Args[1].ResolveTypes();
                } else {
                    throw new System.Exception("Right argument of comparison operator must be a number type or pointer!");
                }
            }

            // Coerce items if needed.
            if (leftType.CanImplicitlyCastTo(rightType)) {
                Args[0] = new ExpressionCast(Args[0], rightType); // Coerce left number to right number.
            } else if (rightType.CanImplicitlyCastTo(leftType)) {
                Args[1] = new ExpressionCast(Args[1], leftType); // Coerce right number to left number.
            } else {
                throw new System.Exception("Unable to coerce math operands!");
            }
            if (Args[0].ReturnType().IsFloatingPoint()) Floating = true;
            else if (Args[0].ReturnType().IsSigned()) Signed = true;
            LValue = false;

        }

        protected override VarType GetReturnTypeDefault() {
            return VarType.TypeBool;
        }

    }

}