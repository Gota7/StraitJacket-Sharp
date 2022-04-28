
using System;
using System.Collections.Generic;
using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Simple primitives.
    public enum SimplePrimitives {
        VariableLength,
        Object,
        Void
    }

    // A simple primitive.
    public class VarTypeSimplePrimitive : VarType {
        public SimplePrimitives Primitive;

        public VarTypeSimplePrimitive(SimplePrimitives type) {
            Type = VarTypeEnum.PrimitiveSimple;
            Primitive = type;
        }

        protected override LLVMTypeRef LLVMType() {
            switch (Primitive) {
                case SimplePrimitives.VariableLength:
                    throw new Exception("TODO!!!");
                case SimplePrimitives.Object:
                    return LLVMTypeRef.CreatePointer(LLVMTypeRef.Int8, 0);
                case SimplePrimitives.Void:
                    return LLVMTypeRef.Void;
                default:
                    throw new Exception("?????????");
            }
        }

        protected override string Mangled() {
            switch (Primitive) {
                case SimplePrimitives.VariableLength:
                    return "l";
                case SimplePrimitives.Object:
                    return "o";
                case SimplePrimitives.Void:
                    return "v";
                default:
                    throw new System.NotImplementedException("???????????");
            }
        }

        public override bool Equals(object obj) {
            if (obj is VarTypeCustom) return Equals((obj as VarTypeCustom).Resolved);
            if (obj is VarTypeSimplePrimitive) {
                var i = obj as VarTypeSimplePrimitive;
                if (i.Constant != Constant) return false;
                if (i.Atomic != Atomic) return false;
                if (i.Volatile != Volatile) return false;
                return i.Primitive == Primitive;
            }
            return false;
        }

        public override int GetHashCode() {
            HashCode hash = new HashCode();
            hash.Add(Type);
            hash.Add(Constant);
            hash.Add(Volatile);
            hash.Add(Atomic);
            hash.Add(Primitive);
            return hash.ToHashCode();
        }

        public override string ToString() {
            string ret = base.ToString();
            switch (Primitive) {
                case SimplePrimitives.VariableLength:
                    ret += "varlen";
                    break;
                case SimplePrimitives.Object:
                    ret += "object";
                    break;
                case SimplePrimitives.Void:
                    ret += "void";
                    break;
                default:
                    throw new Exception("Can't get string for primitive type: " + Primitive.ToString());
            }
            return ret;
        }

        public override bool CanImplicitlyCastTo(VarType other) {
            return base.CanImplicitlyCastTo(other);
        }

        public override Expression DefaultValue() {
            throw new NotImplementedException();
        }

    }

}