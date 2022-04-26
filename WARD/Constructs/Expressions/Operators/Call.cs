using LLVMSharp.Interop;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WARD.Constructs {

    // Call operator.
    public class OperatorCall : OperatorBase {

        // Create a new address of operator.
        public OperatorCall(Expression toCall, params Expression[] args) : base("Call", new Expression[] { toCall }.Concat(args).ToArray()) {}

        protected override void ResolveTypesDefault() {

            // Function that can be called, nothing to do.
            if (Args[0].ReturnType().Type == VarTypeEnum.PrimitiveFunction) {
                // All good.
            }

            // Function pointer, must dereference it.
            else if (Args[0].ReturnType().Type == VarTypeEnum.Pointer && (Args[0].ReturnType() as VarTypePointer).PointedTo.Type == VarTypeEnum.PrimitiveFunction) {
                Args[0] = new OperatorDereference(Args[0]);
                Args[0].ResolveTypes();
            }

            // Can't call expression.
            else {
                throw new Exception("Can only call a function or a function pointer!");
            }

            // TODO: FIND PROPER FUNCTION TO CALL FROM LEFT!!!

        }

        protected override VarType GetReturnTypeDefault() {
            return (Args[0].ReturnType().TrueType() as VarTypeFunction).ReturnType.TrueType();
        }

        protected override LLVMValueRef CompileDefault(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            LLVMValueRef[] args = new LLVMValueRef[Args.Length - 1];
            for (int i = 0; i < args.Length; i++) {
                args[i] = Args[i + 1].CompileRValue(mod, builder, param);
            }
            return builder.BuildCall(Args[0].CompileRValue(mod, builder, param), args);
        }

    }

}