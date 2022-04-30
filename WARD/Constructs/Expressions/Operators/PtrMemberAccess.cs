using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Access a member of a pointer to a struct.
    public class OperatorPtrMemberOf : OperatorBase {
        uint Idx = uint.MaxValue; // -1 or max for use string.
        string Member = ""; // Member name.

        // Access a member of a pointer to a struct.
        public OperatorPtrMemberOf(Expression expr, uint member) : base("PtrMemberOf", expr) {
            Idx = member;
        }

        // Access a member of a pointer to a struct. If the member name begins with # then it will return the implemented struct with the given name.
        public OperatorPtrMemberOf(Expression expr, string member) : base("PtrMemberOf", expr) {
            Member = member;
        }

        protected override void ResolveTypesDefault() {

            // Make sure expression results in pointer to a struct.
            var retType = Args[0].ReturnType();
            if (retType.Type != VarTypeEnum.Pointer || (retType as VarTypePointer).PointedTo.Type != VarTypeEnum.Tuple) {
                throw new System.Exception("Can not take access the pointer member if the input is not a pointer to a structure!");
            }

            // Make the shortcut expression as a->b is the same as (*a).b
            if (Idx == uint.MaxValue) {
                Args[0] = new OperatorMemberOf(new OperatorDereference(Args[0]), Member);
            } else {
                Args[0] = new OperatorMemberOf(new OperatorDereference(Args[0]), Idx);
            }
            Args[0].ResolveTypes();
            LValue = true;

        }

        protected override VarType GetReturnTypeDefault() {
            return new VarTypePointer(Args[0].ReturnType());
        }

        protected override LLVMValueRef CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            return Args[0].CompileLValue(mod, builder, param); // Don't load anything just yet.
        }

    }

}