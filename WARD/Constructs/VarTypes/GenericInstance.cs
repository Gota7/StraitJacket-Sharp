using System;
using System.Collections.Generic;
using System.Linq;
using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Generic instance.
    public class VarTypeGenericInstance : VarType {
        public VarType ToInstance;
        public List<VarType> TypeParameters;
        public VarTypeStruct InstancedStruct {
            get {
                if (m_InstancedStruct == null) GetInstancedStruct();
                return m_InstancedStruct;
            }
            set { m_InstancedStruct = value; }
        }
        VarTypeStruct m_InstancedStruct;

        public VarTypeGenericInstance(VarType toInstance, List<VarType> typeParameters) {
            ToInstance = toInstance;
            TypeParameters = typeParameters;
        }

        // Instance this struct using generic parameters.
        private void GetInstancedStruct() {

        }

        protected override LLVMTypeRef LLVMType() {
            return InstancedStruct.GetLLVMType();
        }

        protected override string Mangled() {
            return InstancedStruct.GetMangled();
        }

        public override bool CanImplicitlyCastTo(VarType other) {
            return InstancedStruct.CanCastTo(other);
        }

        public override bool CanCastTo(VarType other) {
            return InstancedStruct.CanCastTo(other);
        }

        public override ReturnValue CastTo(ReturnValue srcVal, VarType destType, LLVMModuleRef mod, LLVMBuilderRef builder) {
            return InstancedStruct.CastTo(srcVal, destType, mod, builder);
        }

        public override bool Equals(object obj) {
            return InstancedStruct.Equals(obj);
        }
        
        public override int GetHashCode() {
            return InstancedStruct.GetHashCode();
        }

        public override string ToString() {
            string ret = base.ToString() + ToInstance.ToString() + "<";
            foreach (var t in TypeParameters) {
                ret += t.ToString();
                if (t != TypeParameters.Last()) ret += ", ";
            }
            return ret + ">";
        }

        public override Expression DefaultValue() {
            return InstancedStruct.DefaultValue();
        }

    }

}