using System.Collections.Generic;
using WARD.Constructs;
using LLVMSharp.Interop;
using System.Linq;

namespace WARD.Builder {

    // Asylum program builder.
    public partial class ProgramBuilder {
        Dictionary<string, Constructs.AST> ASTs = new Dictionary<string, Constructs.AST>();
        Scope RootScope;
        CodeStatements TopLevel = new CodeStatements();
        CodeStatements CurrStatements;

        // Create a new builder, for compiling EASL or EASL is not used.
        public ProgramBuilder() {
            CurrScope = new Scope() { Name = "" };
            ScopeNum = 0;
            CurrStatements = TopLevel;
            RootScope = Scope();
            Constructs.Scope.Root = RootScope;
        }

        // Create a new builder.
        public ProgramBuilder(Constructs.AST easl, Scope easlScope, int scopeNum) {
            ASTs.Add("EASL", easl);
            CurrScope = easlScope;
            ScopeNum = scopeNum;
            CurrStatements = TopLevel;
            RootScope = Scope();
            Constructs.Scope.Root = RootScope;
        }

        // Compile the code.
        public Dictionary<string, LLVMModuleRef> Compile() {
            Dictionary<string, LLVMModuleRef> ret = new Dictionary<string, LLVMModuleRef>();
            if (TopLevel.Statements.Count > 0) {
                ASTs[TopLevelFile].TopLevel = TopLevel;
            }
            foreach (var s in ASTs.Keys) {
                ASTs[s].PrepareForCompilation();
                var mod = ASTs[s].Compile(s, "");
                if (!s.Equals("EASL")) mod.Verify(LLVMVerifierFailureAction.LLVMPrintMessageAction);
                ret.Add(s, mod);
            }
            return ret;
        }

        // Get output variables for making a new builder.
        public void GetEASLOutVars(out Constructs.AST easl, out Scope easlScope, out int scopeNum) {
            easl = ASTs.Values.ElementAt(0);
            easlScope = CurrScope;
            scopeNum = ScopeNum;
        }

    }

}