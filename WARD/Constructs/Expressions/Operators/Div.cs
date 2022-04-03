using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Divide numbers or pointers.
    public class OperatorDiv : OperatorMath {

        // Make a new division operator.
        public OperatorDiv(Expression left, Expression right) : base(left, right, "Div", true) {}

        protected override ReturnValue CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {

            // Get arguments.
            LLVMValueRef left = Args[0].Compile(mod, builder, param).Val;
            if (Args[0].LValue) left = builder.BuildLoad(left, "tmpLoad");
            LLVMValueRef right = Args[1].Compile(mod, builder, param).Val;
            if (Args[1].LValue) right = builder.BuildLoad(right, "tmpLoad");

            // Compile build.
            VarType retType = Args[0].ReturnType();
            if (retType.IsFloatingPoint()) {
                return new ReturnValue(builder.BuildFDiv(left, right));
            } else {
                throw new System.NotImplementedException();
                //return new ReturnValue(builder.BuildDiv(left, right));
            }

        }

    }

}