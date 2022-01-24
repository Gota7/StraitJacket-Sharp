using System.Collections.Generic;
using StraitJacketLib.Constructs;

namespace StraitJacketLib.Builder {

    // Asylum program builder.
    public partial class ProgramBuilder {
        bool InGenericScope => NumGenericScopes > 0;
        int NumGenericScopes;

        // Make the current structure or function generic.
        public void BeginGenericScope(string genericName, params GenericTypeInfo[] types) {
            throw new System.NotImplementedException();
            //EnterScope("_G" + t.ToString(), true);
            //NumGenericScopes++;
        }

        // End generic scope.
        public void EndGenericScope() {
            if (!InGenericScope) throw new System.Exception("Not in a generic scope!");
            NumGenericScopes--;
            ExitScope();
        }

    }

}