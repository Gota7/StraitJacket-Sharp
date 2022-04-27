using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Bitwise. math.
    public abstract class BitwiseMath : OperatorBase {

        // Create a bitwise math operator.
        public BitwiseMath(string op, params Expression[] args) : base(op, args) {}

        protected override void ResolveTypesDefault() {

            // Make sure operands are integers.
            LValue = false;
            for (int i = 0; i < Args.Length; i++) {
                VarType retType = Args[i].ReturnType();
                if (retType.IsFixed() || retType.IsUnsigned() || retType.IsSigned()) {
                    bool allCastable = true;
                    for (int j = 0; j < Args.Length; j++) {
                        if (i != j) {
                            if (!Args[j].ReturnType().CanImplicitlyCastTo(retType)) {
                                allCastable = false;
                                break;
                            }
                        }
                    }
                    if (allCastable) return;
                }
            }
            throw new System.Exception("Bitwise operands must be integer types!");

        }

        protected override VarType GetReturnTypeDefault() {
            return Args[0].ReturnType();
        }

    }

}