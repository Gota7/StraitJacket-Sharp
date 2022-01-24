using StraitJacketLib.Constructs;

namespace StraitJacketLib {

    // For mangling names. TODO: SOME FORM OF COMPRESSION!
    public static class Mangler {

        public static string ManglePrefix() {
            return "_A";
        }

        public static string MangleScope(Scope s) {
            Scope curr = s;
            string ret = "";
            while (curr != null) {
                if (curr.Name.Length == 0) { curr = curr.Parent; continue; }
                ret = curr.Name.Length + curr.Name + ret;
                curr = curr.Parent;
            }
            return ret;
        }
        
        public static string MangleType(VarType type) {
            return type.GetMangled();
        }

        public static string MangleFunction(Function f) {
            string pars = "";
            for (int i = 0; i < f.Parameters.Count; i++) {
                pars += MangleType(f.Parameters[i].Value.Type);
            }
            string ret = ManglePrefix() + MangleScope(f.Scope) + f.Name.Length + f.Name + "E" + pars;
            if (!f.ReturnType.Equals(new VarTypeSimplePrimitive(SimplePrimitives.Void))) {
                ret += "R" + MangleType(f.ReturnType);
            }
            return ret;
        }

    }

}