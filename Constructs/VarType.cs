using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using LLVMSharp;
using LLVMSharp.Interop;

namespace StraitJacket.Constructs {

    // Number type.
    public enum NumberType {
        Whole,
        Decimal
    }

    // A number.
    public class Number {
        public bool ForceSigned;
        public NumberType Type;
        public long ValueWhole;
        public double ValueDecimal;
        public uint MinBits => ForceSigned ? ((uint)Math.Log2(Math.Abs(ValueWhole)) + 2) : ((uint)Math.Log2(ValueWhole) + 1);
    }

    // Primitives.
    public enum Primitives {
        String,
        Bool,
        Unsigned,
        Signed,
        Half,
        Float,
        Double,
        Extended,
        Decimal,
        Fixed,
        VariableLength,
        Object,
        Void,
        Function,
        Event,
        Char,
        WideChar,
        ToBeDetermined,
        UnsignedAny,
        SignedAny,
        FloatingAny,
        FixedAny
    }

    // Variable type enum
    public enum VarTypeEnum {
        Primitive,
        Tuple,
        Custom,
        Pointer,
        Reference,
        Array,
        Generics
    }

    // Variable type.
    public class VarType : IEqualityComparer<VarType> {
        public string Name;
        public VariableOrFunction ToResolve;
        public VarTypeEnum Type;
        public Primitives Primitive;
        public uint BitWidth;
        public uint FractionWidth;
        public VarType EmbeddedType;
        public VarType[] Members;
        public long ArrayLen;
        public bool Constant;
        public bool Static;
        public bool Volatile;
        public bool Atomic;
        public bool Variadic;
        public bool ContainsStruct;
        public Scope Scope;
        private bool TypeNotGotten = true;
        private LLVMTypeRef GottenType = null;
        public override string ToString() => Mangler.MangleType(this);

        // Disallow external creation.
        private VarType() {}

        // Create a constant type.
        public static VarType AppendConstantToType(VarType varType) {
            varType.Constant = true;
            return varType;
        }

        // Create a variadic type.
        public static VarType AppendVariadicToType(VarType varType) {
            varType.Variadic = true;
            return varType;
        }

        // Create a simple primitive type.
        public static VarType CreatePrimitiveSimple(Primitives primitive) {
            VarType ret = new VarType();
            ret.Type = VarTypeEnum.Primitive;
            ret.Primitive = primitive;
            return ret;
        }

        // Create an integer.
        public static VarType CreateInt(bool signed, uint bitWidth) {
            VarType ret = new VarType();
            ret.Type = VarTypeEnum.Primitive;
            ret.Primitive = signed ? Primitives.Signed : Primitives.Unsigned;
            ret.BitWidth = bitWidth;
            return ret;
        }

        // Create a function.
        public static VarType CreateFunction(VarType retType, List<VarType> parameters) {
            VarType ret = new VarType();
            ret.Type = VarTypeEnum.Primitive;
            ret.Primitive = Primitives.Function;
            ret.EmbeddedType = retType;
            ret.Members = parameters.ToArray();
            return ret;
        }

        // Create a tuple.
        public static VarType CreateTuple(List<VarType> members) {
            VarType ret = new VarType();
            ret.Type = VarTypeEnum.Tuple;
            ret.Members = members.ToArray();
            return ret;
        }

        // Create a reference to a custom type.
        public static VarType CreateCustom(Scope currScope, VariableOrFunction toResolve) {
            VarType ret = new VarType();
            ret.Scope = currScope;
            ret.Type = VarTypeEnum.Custom;
            ret.ToResolve = toResolve;
            return ret;
        }

        // Create a pointer.
        public static VarType CreatePointer(VarType toPointTo) {
            VarType ret = new VarType();
            ret.Type = VarTypeEnum.Pointer;
            ret.EmbeddedType = toPointTo;
            return ret;
        }

        // Create a reference.
        public static VarType CreateReference(VarType toPointTo) {
            VarType ret = new VarType();
            ret.Type = VarTypeEnum.Reference;
            ret.EmbeddedType = toPointTo;
            return ret;
        }

        // TODO!!!
        public LLVMTypeRef GetLLVMType() {
            if (TypeNotGotten) {
                switch (Type)
                {
                    case VarTypeEnum.Primitive:
                        switch (Primitive) {
                            case Primitives.String:
                                GottenType = LLVMTypeRef.CreatePointer(LLVMTypeRef.Int8, 0);
                                break;
                            case Primitives.Bool:
                                GottenType = LLVMTypeRef.Int1;
                                break;
                            case Primitives.Unsigned:
                                GottenType = LLVMTypeRef.CreateInt(BitWidth);
                                break;
                            case Primitives.Signed:
                                GottenType = LLVMTypeRef.CreateInt(BitWidth);
                                break;
                            case Primitives.Half:
                                GottenType = LLVMTypeRef.Half;
                                break;
                            case Primitives.Float:
                                GottenType = LLVMTypeRef.Float;
                                break;
                            case Primitives.Double:
                                GottenType = LLVMTypeRef.Double;
                                break;
                            case Primitives.Extended:
                                GottenType = LLVMTypeRef.X86FP80;
                                break;
                            case Primitives.Decimal:
                                GottenType = LLVMTypeRef.FP128;
                                break;
                            case Primitives.Fixed:
                                break;
                            case Primitives.VariableLength:
                                break;
                            case Primitives.Object:
                                GottenType = LLVMTypeRef.CreatePointer(LLVMTypeRef.Int8, 0);
                                break;
                            case Primitives.Void:
                                GottenType = LLVMTypeRef.Void;
                                break;
                            case Primitives.Function:
                                var funcParams = new LLVMTypeRef[Members.Length];
                                for (int i = 0; i < Members.Length; i++) {
                                    funcParams[i] = Members[i].GetLLVMType();
                                }
                                GottenType = LLVMTypeRef.CreatePointer(LLVMTypeRef.CreateFunction(EmbeddedType.GetLLVMType(), funcParams, funcParams.Length == 0 ? false : Members.Last().Variadic), 0);
                                break;
                            case Primitives.Event:
                                break;
                            case Primitives.Char:
                                GottenType = LLVMTypeRef.Int8;
                                break;
                            case Primitives.WideChar:
                                GottenType = LLVMTypeRef.Int16;
                                break;
                            case Primitives.ToBeDetermined:
                                break;
                            case Primitives.UnsignedAny:
                                break;
                            case Primitives.SignedAny:
                                break;
                            case Primitives.FloatingAny:
                                break;
                            case Primitives.FixedAny:
                                break;
                        }
                        break;
                    case VarTypeEnum.Tuple:
                        break;
                    case VarTypeEnum.Custom:
                        GottenType = Scope.ResolveType(ToResolve).GetLLVMType();
                        break;
                    case VarTypeEnum.Pointer:
                        GottenType = LLVMTypeRef.CreatePointer(EmbeddedType.GetLLVMType(), 0);
                        break;
                    case VarTypeEnum.Reference:
                        break;
                    case VarTypeEnum.Array:
                        break;
                    case VarTypeEnum.Generics:
                        break;
                }
                TypeNotGotten = false;
            }
            if (GottenType == null) throw new Exception("BAD TYPE!!!");
            return GottenType;
        }

        // If two types are equal.
        public bool Equals(VarType x, VarType y)
        {

            // Matching type.
            if (x.Type == y.Type) {

                // Primitives. TODO (type isn't enough)!!!
                if (x.Type == VarTypeEnum.Primitive) {
                    return x.Primitive == y.Primitive;
                }

                // Tuple.
                if (x.Type == VarTypeEnum.Tuple) {
                    if (x.Members.Length == y.Members.Length) {
                        for (int i = 0; i < x.Members.Length; i++) {
                            if (x.Members[i] != y.Members[i]) return false;
                        }
                    }
                }

                // Custom.
                if (x.Type == VarTypeEnum.Custom) {
                    // TODO!!!
                    return true;
                }

                // Raw pointer.
                if (x.Type == VarTypeEnum.Pointer) {
                    return x.EmbeddedType == y.EmbeddedType;
                }

                // Reference.
                if (x.Type == VarTypeEnum.Reference) {
                    return x.EmbeddedType == y.EmbeddedType;
                }

                // Array.
                if (x.Type == VarTypeEnum.Array) {
                    return x.ArrayLen == y.ArrayLen && x.EmbeddedType == y.EmbeddedType;
                }

                // Generics.
                if (x.Type == VarTypeEnum.Generics) {
                    // TODO!!!
                    return true;
                }

            }

            // Type with a different reference.
            if (x.Type == VarTypeEnum.Custom || y.Type == VarTypeEnum.Custom) {
                // TODO!!!
                return true;
            }

            // Can't be the same type.
            return false;

        }

        // Hash code for types.
        public int GetHashCode([DisallowNull] VarType obj)
        {

            // Primitive.
            if (obj.Type == VarTypeEnum.Primitive) {
                // TODO!!!
            }
            
            // Unknown.
            return 0;

        }

    }

    // Variable parameter.
    public class VarParameter {
        public string Label;
        public Variable Value = new Variable();
        public List<LLVMValueRef> VariadicArgs;
    }

}