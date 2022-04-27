using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Boolean math.
    public abstract class BooleanMath : OperatorBase {

        // Create a boolean math operator.
        public BooleanMath(string op, params Expression[] args) : base(op, args) {}

        protected override void ResolveTypesDefault() {

            // Make sure all arguments are booleans.
            for (int i = 0; i < Args.Length; i++) {
                VarType retType = Args[i].ReturnType();
                VarType boolType = new VarTypeSimplePrimitive(SimplePrimitives.Bool);
                if (!retType.Type.Equals(boolType)) {
                    if (retType.CanImplicitlyCastTo(boolType)) {
                        Args[i] = new ExpressionCast(Args[i], boolType);
                        Args[i].ResolveTypes();
                    } else {
                        throw new System.Exception("Can not use boolean expression on non-boolean operands!");
                    }
                }
            }

        }

        protected override VarType GetReturnTypeDefault() {
            return new VarTypeSimplePrimitive(SimplePrimitives.Bool); // Can only return booleans.
        }

    }

}