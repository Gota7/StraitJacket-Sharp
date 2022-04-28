namespace WARD.Constructs {

    // Typedef extensions.
    public partial class VarType {
        public static VarType TypeBool => new VarTypeInteger(false, 1);
        public static VarType TypeChar => new VarTypeInteger(true, 8);
        public static VarType TypeWideChar => new VarTypeInteger(true, 16);
        public static VarType TypeConstString => new VarTypePointer(TypeChar) { Constant = true };
        public static VarType TypeObject => new VarTypeSimplePrimitive(SimplePrimitives.Object);
        public static VarType TypeVoid => new VarTypeSimplePrimitive(SimplePrimitives.Void);
        public static VarType TypePtrAsInt => new VarTypeInteger(false, 64);
    }

}