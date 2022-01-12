using System.Collections.Generic;
using System.Linq;
using StraitJacketLib.Constructs;

namespace StraitJacketLib.Builder {

    // Asylum program builder.
    public partial class ProgramBuilder {
        Scope OldScope;
        Implementation CurrImplementation = null;

        // Begin an implementation block.
        public void BeginImplementation(VarType type, VarType implements = null) {
            if (CurrImplementation != null) throw new System.Exception("Already in an implementation!");

            // Backup the current scope, and go to the root scope to add the implementation.
            OldScope = CurrScope;
            CurrScope = RootScope;
            EnterScope(Mangler.MangleType(type));

            // Setup new implementation.
            CurrImplementation = new Implementation();
            CurrImplementation.Type = type;
            CurrImplementation.InterfaceToImplement = implements;

        }

        // End an implementation block.
        public void EndImplementation() {

            // Fix the scope.
            ExitScope();
            CurrScope = OldScope;
            CurrImplementation = null;

        }

        // Define struct members to be in the current scope.
        private void DefineMembersInScope(List<VarParameter> existingParameters) {
            if (CurrImplementation.Type.TrueType() as VarTypeStruct != null) {
                var s = CurrImplementation.Type.TrueType() as VarTypeStruct;
                foreach (var e in s.Entries) {
                    if (existingParameters.Where(x => x.Value.Name.Equals(e.Var.Name)).Count() < 1) {
                        Scope().AddVar(e.Var.Name, Variable(e.Var.Type, "this." + e.Var.Name)); // Implicit member of hack.
                    }
                }
            }
        }

        // Define a function.
        public void BeginImplFunction(string name, VarType returnType, List<VarParameter> parameters) {
            parameters.Insert(0, new VarParameter() { Value = Variable(new VarTypePointer(ThisType()), "this") }); // Add this parameter.
            BeginFunction(name, returnType, parameters);
            if (!CurrFunction.Static) CurrFunction.ThisCall = true;
            DefineMembersInScope(parameters);
        }

        // Define a constructor.
        public void BeginImplConstructor(string name, List<VarParameter> parameters) {
            BeginFunction(name, ThisType(), parameters);
            VariableDefinition(null, Variable(ThisType(), "this")); // Define this.
            DefineMembersInScope(parameters);
        }

        // End a constructor.
        public void EndImplConstructor() {
            Code(new ReturnStatement(new ExpressionVariable(VariableOrFunction("this")))); // A constructor will always return this.
            EndFunction();
        }

        // Define a destructor. TODO!!!
        public void BeginImplDestructor() {

        }

        // End a destructor. TODO!!!
        public void EndImplDestructor() {

        }

        // Define a cast. TODO!!!
        public void BeginImplCast(bool implicitCast) {

        }

        // Define an operator. TODO!!!
        public void BeginImplOperator() {

        }

        // This type.
        public VarType ThisType() {
            if (CurrImplementation == null) throw new System.Exception("Currently not in an implementation block!");
            return CurrImplementation.Type;
        }

    }

}