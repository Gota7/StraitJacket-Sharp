using System;
using System.Collections.Generic;
using System.Linq;
using LLVMSharp.Interop;

namespace StraitJacketLib.Constructs {

    // Function type.
    public class VarTypeFunction : VarType {
        public VarType ReturnType;
        public List<VarType> Parameters;
        public Function FoundFunc; // If the function is existing and found. Note that this only used for function calls and doesn't change equality.

        public VarTypeFunction(VarType retType, List<VarType> parameters) {
            Type = VarTypeEnum.PrimitiveFunction;
            ReturnType = retType;
            Parameters = parameters;
        }

        protected override LLVMTypeRef LLVMType() {
            List<LLVMTypeRef> parameters = new List<LLVMTypeRef>();
            bool variadic = false;
            foreach (var m in Parameters) {
                parameters.Add(m.GetLLVMType());
            }
            if (parameters.Count > 0) {
                var last = Parameters.Last();
                if (last.Variadic) {
                    variadic = true;
                    parameters.Remove(parameters.Last());
                }
            }
            return LLVMTypeRef.CreateFunction(ReturnType.GetLLVMType(), parameters.ToArray(), variadic);
        }

        protected override string Mangled() {
            string func = "";
            func += Mangler.MangleType(ReturnType);
            foreach (var m in Parameters) {
                func += Mangler.MangleType(m);
            }
            return "n" + func;
        }

        public override bool Equals(object obj) {
            if (obj is VarTypeCustom) return Equals((obj as VarTypeCustom).Resolved);
            if (obj is VarTypeFunction) {
                var i = obj as VarTypeFunction;
                if (i.Constant != Constant) return false;
                if (i.Atomic != Atomic) return false;
                if (i.Volatile != Volatile) return false;
                if (i.Parameters.Count != Parameters.Count) return false;
                for (int j = 0; j < i.Parameters.Count; j++) {
                    if (!i.Parameters[j].Equals(Parameters[j])) return false;
                }
                return i.ReturnType.Equals(ReturnType);
            }
            return false;
        }
        
        public override int GetHashCode() {
            HashCode hash = new HashCode();
            hash.Add(Type);
            hash.Add(Constant);
            hash.Add(Volatile);
            hash.Add(Atomic);
            hash.Add(ReturnType.GetHashCode());
            hash.Add(Parameters.GetHashCode());
            return hash.ToHashCode();
        }

        public override string ToString() {
            string ret = base.ToString() + "func<" + ReturnType;
            foreach (var p in Parameters) {
                ret += ", " + p.ToString();
            }
            return ret + ">";
        }

        public override Expression DefaultValue() {
            throw new NotImplementedException();
        }

        // See if a function type can be made from another.
        public bool CanBeConvertedFrom(List<VarType> paramTypes, VarType expectedReturnType = null) {

            // Return type doesn't match.
            if (expectedReturnType != null && !expectedReturnType.CanCastTo(ReturnType)) {
                return false;
            }

            // Go through each parameter.
            if (Parameters.Count - 1 > paramTypes.Count) return false; // Not enough parameters given.
            if (Parameters.Count > 0 && !Parameters.Last().Variadic && Parameters.Count > paramTypes.Count) return false; // Not enough parameters given even when not variadic.
            for (int i = 0; i < paramTypes.Count; i++) {
                // TODO!!!!!!!!!
            }

            // Can convert.
            return true;

        }

    }

}