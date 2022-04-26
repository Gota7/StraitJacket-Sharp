using System.Collections.Generic;
using System.Linq;
using LLVMSharp;
using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Allocate an array of values. TODO: CONSTRUCT TOTAL SIZE BY MULTIPLYING RESULTS OF LENGTHS!!!
    public class ExpressionArrayAllocate : Expression {
        public VarType EmbeddedType;
        public List<Expression> Lengths;
        Expression TotalSize;

        public ExpressionArrayAllocate(VarType embeddedType, List<Expression> lengths) {
            Type = ExpressionType.ArrayAllocate;
            EmbeddedType = embeddedType;
            Lengths = lengths;
            LValue = false;
        }

        public override void ResolveVariables() {
            TotalSize.ResolveVariables(); // Lengths will have this called.
        }

        public override void ResolveTypes(VarType preferredReturnType, List<VarType> parameterTypes) {
            TotalSize.ResolveTypes(preferredReturnType, parameterTypes);
        }

        public override VarType GetReturnType() {
            List<uint> lens = new List<uint>();
            foreach (var l in Lengths) {
                lens.Add(0);
            }
            return new VarTypeArray(EmbeddedType, lens);
        }

        public override LLVMValueRef Compile(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {
            LLVMValueRef totalSize = TotalSize.Compile(mod, builder, param);
            if (TotalSize.LValue) totalSize = builder.BuildLoad(totalSize, "SJ_Load");
            return builder.BuildArrayAlloca
            (
                EmbeddedType.GetLLVMType(),
                totalSize,
                "SJ_BuildArrayAlloca"
            );
        }

    }

}