using System;
using System.Collections.Generic;
using System.Linq;
using LLVMSharp.Interop;

namespace WARD.Constructs {

    // A tuple.
    public class VarTypeTuple : VarType {
        public List<VarType> Members;
        public bool IsVector { get; private set; }

        public VarTypeTuple(List<VarType> members) {
            Type = VarTypeEnum.Tuple;
            Members = members;
            for (int i = 1; i < Members.Count; i++) {
                if (!Members[i].Equals(Members[0])) {
                    return;
                }
            }
            IsVector = true;
        }

        protected override LLVMTypeRef LLVMType() {
            if (IsVector) {
                return LLVMTypeRef.CreateVector(Members[0].GetLLVMType(), (uint)Members.Count());
            } else {
                List<LLVMTypeRef> members = new List<LLVMTypeRef>();
                foreach (var m in Members) {
                    members.Add(m.GetLLVMType());
                }
                return LLVMTypeRef.CreateStruct(members.ToArray(), true);
            }
        }

        protected override string Mangled() {
            string tpl = "";
            foreach (var m in Members) {
                tpl += Mangler.MangleType(m);
            }
            return "T" + tpl;
        }

        public override bool Equals(object obj) {
            if (obj is VarTypeCustom) return Equals((obj as VarTypeCustom).Resolved);
            if (obj is VarTypeTuple) {
                var i = obj as VarTypeTuple;
                if (i.Constant != Constant) return false;
                if (i.Atomic != Atomic) return false;
                if (i.Volatile != Volatile) return false;
                if (i.Members.Count != Members.Count) return false;
                for (int j = 0; j < i.Members.Count; j++) {
                    if (!i.Members[j].Equals(Members[j])) return false;
                }
                return i.IsVector == IsVector;
            }
            return false;
        }
        
        public override int GetHashCode() {
            HashCode hash = new HashCode();
            hash.Add(Type);
            hash.Add(Constant);
            hash.Add(Volatile);
            hash.Add(Atomic);
            hash.Add(Members.GetHashCode());
            hash.Add(IsVector.GetHashCode());
            return hash.ToHashCode();
        }

        public override string ToString() {
            string ret = base.ToString() + "(";
            foreach (var m in Members) {
                ret += m.ToString();
                if (m != Members.Last()) {
                    ret += ", ";
                }
            }
            return ret + ")";
        }

        public override Expression DefaultValue() {
            List<Expression> exps = new List<Expression>();
            foreach (var m in Members) {
                exps.Add(m.DefaultValue());
            }
            return new ExpressionComma(exps);
        }

    }

}