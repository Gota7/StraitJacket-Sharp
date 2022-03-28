using System;
using LLVMSharp.Interop;

namespace StraitJacketLib.Constructs {

    // References another type.
    public class VarTypeReference : VarType {
        public VarType ReferencedTo;
        
        public VarTypeReference(VarType referencedTo) {
            Type = VarTypeEnum.Pointer;
            ReferencedTo = referencedTo;
        }

        protected override LLVMTypeRef LLVMType() {
            return LLVMTypeRef.CreatePointer(ReferencedTo.GetLLVMType(), 0);
        }

        protected override string Mangled() => "P" + Mangler.MangleType(ReferencedTo);

        public override bool Equals(object obj) {
            if (obj is VarTypeCustom) return Equals((obj as VarTypeCustom).Resolved);
            if (obj is VarTypeReference) {
                var i = obj as VarTypeReference;
                if (i.Constant != Constant) return false;
                if (i.Atomic != Atomic) return false;
                if (i.Volatile != Volatile) return false;
                return i.ReferencedTo.Equals(ReferencedTo);
            }
            return false;
        }
        
        public override int GetHashCode() {
            HashCode hash = new HashCode();
            hash.Add(Type);
            hash.Add(Constant);
            hash.Add(Volatile);
            hash.Add(Atomic);
            hash.Add(ReferencedTo.GetHashCode());
            return hash.ToHashCode();
        }

        public override string ToString() {
            return base.ToString() + ReferencedTo.ToString() + "&";
        }

        public override Expression DefaultValue() {
            return new ExpressionConstInt(false, 0);
        }

    }

}