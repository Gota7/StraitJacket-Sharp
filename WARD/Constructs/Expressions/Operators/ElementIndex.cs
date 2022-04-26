using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Element index operator.
    public class OperatorElementIndex : OperatorBase {

        // Create a new address of operator.
        public OperatorElementIndex(Expression expr, Expression index, string opName = "ElementIndex") : base(opName, expr, index) {}

        protected override void ResolveTypesDefault() {

            // Make sure expression type is pointer, struct, or array and index is an integer.
            VarType exprType = Args[0].ReturnType();
            VarType indexType = Args[1].ReturnType();
            if (exprType.Type != VarTypeEnum.Pointer && exprType.Type != VarTypeEnum.Tuple && exprType.Type != VarTypeEnum.Array) {
                throw new System.Exception("Can't index the given expression!");
            }
            if (!indexType.IsUnsigned() && !indexType.IsSigned()) {
                throw new System.Exception("Can't index expression with non-integer type!");
            }
            LValue = false;

        }

        protected override VarType GetReturnTypeDefault() {
            throw new System.NotImplementedException();
        }

        protected override LLVMValueRef CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            throw new System.NotImplementedException();
        }

    }

}