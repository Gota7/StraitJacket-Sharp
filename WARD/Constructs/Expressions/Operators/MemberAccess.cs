using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Access a member of a struct.
    public class OperatorMemberOf : OperatorBase {
        uint Idx = uint.MaxValue; // -1 or max for use string.
        string Member = ""; // Member name.

        // Access a member of a struct.
        public OperatorMemberOf(Expression expr, uint member) : base("MemberOf", expr) {
            Idx = member;
        }

        // Access a member of a struct. If the member name begins with # then it will return the implemented struct with the given name.
        public OperatorMemberOf(Expression expr, string member) : base("MemberOf", expr) {
            Member = member;
        }

        protected override void ResolveTypesDefault() {

            // Make sure expression results in struct.
            var retType = Args[0].ReturnType();
            if (retType.Type != VarTypeEnum.Tuple) {
                throw new System.Exception("Can not take the member of a non-structure!");
            }

            // Resolve member name to an Idx if needed.
            if (Idx == uint.MaxValue) {
                if (!(retType as VarTypeTuple).IsStruct) {
                    throw new System.Exception("Can not take a named member of a structure without member names!");
                }
                Idx = (retType as VarTypeStruct).GetMemberIdx(Member);
            }

            // A member is always an L-value as no loading is done.
            LValue = true;

        }

        protected override VarType GetReturnTypeDefault() {
            return new VarTypePointer(Args[0].ReturnType());
        }

        protected override LLVMValueRef CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            return builder.BuildStructGEP(Args[0].CompileRValue(mod, builder, param), Idx, Member);
        }

    }

}