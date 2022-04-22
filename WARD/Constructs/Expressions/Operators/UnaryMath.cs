namespace WARD.Constructs {

    // Unary math operator with integers or pointers.
    public abstract class UnaryMath : OperatorBase {
        protected bool Numeric;
        private bool AllowPointers;

        // Create a new math operator expression.
        public UnaryMath(Expression expr, string opName, bool allowPointers = false) : base(opName, expr) {
            AllowPointers = allowPointers;
        }

        protected override void ResolveTypesDefault() {

            // Make sure types are numeric or pointers.
            VarType type = Args[0].ReturnType();
            if (type.IsNumeric()) {
                Numeric = true;
            } else if (!AllowPointers) {
                throw new System.Exception("Argument of math operator must be a number type!");
            } else if (type.Type != VarTypeEnum.Pointer) {
                throw new System.Exception("Argument of math operator must be a number type or pointer!");
            }
            LValue = false;

        }

        protected override VarType GetReturnTypeDefault() {
            return Args[0].ReturnType();
        }

    }

}