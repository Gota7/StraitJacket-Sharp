using System;
using System.Collections.Generic;
using LLVMSharp.Interop;

namespace StraitJacketLib.Constructs {

    // An array of data. Since arrays can be variable length, the under the hood type is a pointer to an array.
    public class VarTypeArray : VarType {
        public VarType EmbeddedType;
        public List<uint> Lengths = new List<uint>();
        
        public VarTypeArray(VarType embeddedType, List<uint> lengths) {
            Type = VarTypeEnum.Array;
            EmbeddedType = embeddedType;
            Lengths = lengths;
        }

        protected override LLVMTypeRef LLVMType() {
            uint arrSize = 0;
            foreach (var l in Lengths) {
                if (arrSize == 0 && l > 0) arrSize = 1;
                arrSize *= l;
            }
            return LLVMTypeRef.CreatePointer(EmbeddedType.GetLLVMType(), 0); // An array is just a pointer to data.
        }

        public override bool Equals(object obj) {
            if (obj is VarTypeCustom) return Equals((obj as VarTypeCustom).Resolved);
            if (obj is VarTypeArray) {
                var i = obj as VarTypeArray;
                if (i.Constant != Constant) return false;
                if (i.Atomic != Atomic) return false;
                if (i.Volatile != Volatile) return false;
                if (i.Variadic != Variadic) return false;
                if (!i.EmbeddedType.Equals(EmbeddedType)) return false;
                if (i.Lengths.Count != Lengths.Count) return false;
                for (int j = 0; j < i.Lengths.Count; j++) {
                    if (i.Lengths[j] != Lengths[j]) return false;
                }
                return true;
            }
            return false;
        }
        
        public override int GetHashCode() {
            HashCode hash = new HashCode();
            hash.Add(Type);
            hash.Add(Constant);
            hash.Add(Volatile);
            hash.Add(Atomic);
            hash.Add(Variadic);
            hash.Add(EmbeddedType.GetHashCode());
            hash.Add(Lengths.Count.GetHashCode());
            foreach (var l in Lengths) {
                hash.Add(l);
            }
            return hash.ToHashCode();
        }

        public override string ToString() {
            string ret = base.ToString() + EmbeddedType.ToString() + "[";
            for (int i = 0; i < Lengths.Count - 1; i++) {
                ret += ",";
            }
            return ret + "]";
        }

        public override Expression DefaultValue() {
            bool allZeroes = true;
            foreach (var l in Lengths) {
                if (l != 0) allZeroes = false;
            }
            if (allZeroes) {
                throw new System.NotImplementedException(); // NULL POINTER.
            } else {
                return new ExpressionConstArrayAllocate(EmbeddedType, Lengths);
            }
        }

    }

}