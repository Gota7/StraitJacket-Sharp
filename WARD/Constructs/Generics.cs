namespace WARD.Constructs {

    // Type operation.
    public enum TypeInfoOperation {
        Type,
        TypeAnd,
        TypeOr
    }

    // Generic type information.
    public class GenericTypeInfo {
        public TypeInfoOperation Type;
        public IGenericTypeInfo TypeInfo;
    }

    public interface IGenericTypeInfo {}

    public class GenericTypeInfoAnd : IGenericTypeInfo {
        public IGenericTypeInfo Left;
        public IGenericTypeInfo Right;
    }

    public class GenericTypeInfoOr : IGenericTypeInfo {
        public IGenericTypeInfo Left;
        public IGenericTypeInfo Right;
    }

    public class GenericTypeInfoType : IGenericTypeInfo {
        public VarType Implements;
    }

}