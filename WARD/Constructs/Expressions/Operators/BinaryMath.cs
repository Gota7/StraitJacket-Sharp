namespace WARD.Constructs {

    // Binary math operator with integers or pointers.
    public abstract class BinaryMath : OperatorBase {
        protected bool LeftNumeric;
        protected bool RightNumeric;
        private bool AllowPointers;

        // Create a new math operator expression.
        public BinaryMath(Expression left, Expression right, string opName, bool allowPointers = false) : base(opName, left, right) {
            AllowPointers = allowPointers;
        }

        protected override void ResolveTypesDefault() {

            // Make sure types are numeric or pointers, but both can not be pointers.
            VarType leftType = Args[0].ReturnType();
            if (leftType.IsNumeric()) {
                LeftNumeric = true;
            } else if (!AllowPointers) {
                throw new System.Exception("Left argument of math operator must be a number type!");
            } else if (leftType.Type != VarTypeEnum.Pointer) {
                throw new System.Exception("Left argument of math operator must be a number type or pointer!");
            }
            VarType rightType = Args[1].ReturnType();
            if (rightType.IsNumeric()) {
                RightNumeric = true;
            } else if (!AllowPointers) {
                throw new System.Exception("Right argument of math operator must be a number type!");
            } else if (rightType.Type != VarTypeEnum.Pointer) {
                throw new System.Exception("Right argument of math operator must be a number type or pointer!");
            }

            // Both pointers, stop.
            if (!LeftNumeric && !RightNumeric) {
                throw new System.Exception("At least one operand to a math operator has to be a number type!");
            }

            // Coerce items if needed.
            if (!LeftNumeric) {
                Args[1] = new ExpressionCast(Args[1], leftType); // Cast right expression to pointer.
            } else if (!RightNumeric) {
                Args[0] = new ExpressionCast(Args[0], rightType); // Cast left expression to pointer.
            } else if (leftType.CanImplicitlyCastTo(rightType)) {
                Args[0] = new ExpressionCast(Args[0], rightType); // Coerce left number to right number.
            } else if (rightType.CanImplicitlyCastTo(leftType)) {
                Args[1] = new ExpressionCast(Args[1], leftType); // Coerce right number to left number.
            } else {
                throw new System.Exception("Unable to coerce math operands!");
            }
            LValue = false;

        }

        protected override VarType GetReturnTypeDefault() {
            return Args[0].ReturnType();
        }

    }

}