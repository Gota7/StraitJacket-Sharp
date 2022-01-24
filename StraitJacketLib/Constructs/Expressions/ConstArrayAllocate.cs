using System.Collections.Generic;
using System.Linq;
using LLVMSharp;
using LLVMSharp.Interop;

namespace StraitJacketLib.Constructs {

    // Allocate a constant array of values.
    public class ExpressionConstArrayAllocate : Expression {
        public VarType EmbeddedType;
        public List<uint> Lengths;

        public ExpressionConstArrayAllocate(VarType embeddedType, List<uint> lengths) {
            EmbeddedType = embeddedType;
            Lengths = lengths;
            LValue = false;
        }

        public override VarType GetReturnType() {
            return new VarTypeArray(EmbeddedType, Lengths);
        }

        public override bool IsPlural() {
            return false;
        }

        public override void StoreSingle(ReturnValue src, ReturnValue dest, VarType srcType, VarType destType, LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            throw new System.Exception("??????");
        }

        public override void StorePlural(ReturnValue src, ReturnValue dest, VarType srcType, VarType destType, LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            throw new System.Exception("??????");
        }

        public override ReturnValue Compile(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            uint arrSize = 0;
            foreach (var l in Lengths) {
                if (arrSize == 0 && l > 0) arrSize = 1;
                arrSize *= l;
            }
            ExpressionConstInt len = new ExpressionConstInt(false, arrSize);
            return new ReturnValue(builder.BuildArrayAlloca(
                EmbeddedType.GetLLVMType(),
                len.Compile(mod, builder, param).Val,
                "SJ_BuildArrayAlloca"
            ));
        }

    }

}