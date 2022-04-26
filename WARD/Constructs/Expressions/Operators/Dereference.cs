using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Unary dereference operator.
    public class OperatorDereference : OperatorBase {

        // Create a dereference operator.
        public OperatorDereference(Expression expr) : base("Dereference", expr) {}

        /*

            The dereference operator is a bit confusing, so let's look at how it behaves in C:

            int a = 7;
            int* b = &a;

            // * operator returns address 345 as an L-value so it is loaded from.
            int c = *(int*)345;

            // First b's value is loaded (its pointer) into an R-value. The * operator then treats the R-value as an L-value so it can be loaded from again.
            int d = *b;

            // * operator returns address 678 as an L-value so data can be stored in it.
            *(int*)678 = a;

            // First b's value is loaded (its pointer) into an R-value. The * operator then treats this R-value as an L-value so data can be stored to it.
            *b = 3;

            The moral of the story is that all the dereference operator does is force something to be an L-value.
            You can also only use it on pointers, and it returns the pointed type.

        */

        protected override void ResolveTypesDefault() {

            // Make sure argument is a pointer.
            if (Args[0].ReturnType().Type != VarTypeEnum.Pointer) {
                throw new System.Exception("Can't dereference a non-pointer!");
            }
            LValue = true;

        }

        protected override VarType GetReturnTypeDefault() {
            return (Args[0].ReturnType() as VarTypePointer).PointedTo.TrueType();
        }

        protected override LLVMValueRef CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            return Args[0].CompileRValue(mod, builder, param); // Yes, we dereference from an R-value. If it were a pointer variable you would get its value.
        }

    }

}