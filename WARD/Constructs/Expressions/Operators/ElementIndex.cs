using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Element index operator.
    public class OperatorElementIndex : OperatorBase {
        ElementIndexType IndexType;
        VarType ExprType;

        // Element index type.
        enum ElementIndexType {
            Pointer,
            Struct,
            Array
        }

        // Create a new element index operator.
        public OperatorElementIndex(Expression expr, Expression index) : base("ElementIndex", expr, index) {}

        protected override void ResolveTypesDefault() {

            // Make sure expression type is pointer, struct, or array and index is an integer.
            VarType ExprType = Args[0].ReturnType();
            VarType indexType = Args[1].ReturnType();
            if (ExprType.Type != VarTypeEnum.Pointer && ExprType.Type != VarTypeEnum.Tuple && ExprType.Type != VarTypeEnum.Array) {
                throw new System.Exception("Can't index the given expression!");
            }
            if (!indexType.IsUnsigned() && !indexType.IsSigned()) {
                throw new System.Exception("Can't index expression with non-integer type!");
            }
            LValue = true; // We actually do no loading.

            // Get index type.
            switch (ExprType.Type) {
                case VarTypeEnum.Pointer:
                    IndexType = ElementIndexType.Pointer;
                    break;
                case VarTypeEnum.Tuple:
                    IndexType = ElementIndexType.Struct;
                    break;
                case VarTypeEnum.Array:
                    IndexType = ElementIndexType.Array;
                    break;
            }

        }

        protected override VarType GetReturnTypeDefault() {
            switch (IndexType) {
                case ElementIndexType.Pointer:
                    return (ExprType as VarTypePointer).PointedTo.TrueType();
                case ElementIndexType.Struct:
                    return new VarTypeSimplePrimitive(SimplePrimitives.Object); // Dynamically indexing struct can be wack.
                case ElementIndexType.Array:
                    return (ExprType as VarTypeArray).EmbeddedType.TrueType();
                default:
                    return null;
            }
        }

        protected override LLVMValueRef CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            return builder.BuildGEP(
                Args[0].CompileRValue(mod, builder, param),
                new LLVMValueRef[] { Args[1].CompileRValue(mod, builder, param) },
                "SJ_ElementIndex"
            );
        }

    }

}