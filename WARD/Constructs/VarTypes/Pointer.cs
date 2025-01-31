using System;
using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Points to more data.
    public class VarTypePointer : VarType {
        public VarType PointedTo;
        
        public VarTypePointer(VarType pointedTo) {
            Type = VarTypeEnum.Pointer;
            PointedTo = pointedTo;
        }

        protected override LLVMTypeRef LLVMType() {
            return LLVMTypeRef.CreatePointer(PointedTo.GetLLVMType(), 0);
        }

        protected override string Mangled() => "p" + Mangler.MangleType(PointedTo);

        public override bool Equals(object obj) {
            if (obj is VarTypeCustom) return Equals((obj as VarTypeCustom).Resolved);
            if (obj is VarTypePointer) {
                var i = obj as VarTypePointer;
                if (i.Constant != Constant) return false;
                if (i.Atomic != Atomic) return false;
                if (i.Volatile != Volatile) return false;
                return i.PointedTo.Equals(PointedTo);
            }
            return false;
        }
        
        public override int GetHashCode() {
            HashCode hash = new HashCode();
            hash.Add(Type);
            hash.Add(Constant);
            hash.Add(Volatile);
            hash.Add(Atomic);
            hash.Add(PointedTo.GetHashCode());
            return hash.ToHashCode();
        }

        public override string ToString() {
            return base.ToString() + PointedTo.ToString() + "*";
        }

        public override Expression DefaultValue() {
            return new ExpressionConstInt(false, 0);
        }

    }

}