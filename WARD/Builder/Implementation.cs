using System.Collections.Generic;
using System.Linq;
using WARD.Constructs;

namespace WARD.Builder {

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
                        Scope().AddVar(e.Var.Name, new Variable() {
                            Scope = Scope(),
                            Type = e.Var.Type,
                            Name = "this." + e.Var.Name,
                            ScopeOverwriteable = true
                        }); // Implicit member of hack.
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
        public void BeginImplConstructor(List<VarParameter> parameters) {
            BeginFunction("__ct", ThisType(), parameters);
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
        public void BeginImplOperator(Operator op, bool assign, VarType returnType, List<VarParameter> parameters) {
            if (assign) {
                if (parameters.Count() != 1) {
                    throw new System.Exception("Invalid number of parameters for overloading the assignment operator!");
                }
                BeginImplFunction("__opEq" + op.ToString(), returnType, parameters); // Assignments are not static.
            }
            /*switch (op) {
                case Operator.Add:
                    CheckLen(2);
                    break;
                default:
                    throw new System.NotImplementedException("Operator overload for " + op + " not implemented!");
            }*/ // TODO!!!
            if (op == Operator.Inc || op == Operator.Dec) {
                BeginImplFunction("__op" + op.ToString(), returnType, parameters); // Increment and decrement are not static.
            } else {
                BeginFunction("__op" + op.ToString(), returnType, parameters); // Rest are static.
            }
            void CheckLen(int len) {
                if (parameters.Count() != len) throw new System.Exception("Wrong number of parameters for " + op + ". Expected " + len + ".");
            }
        }

        // This type.
        public VarType ThisType() {
            if (CurrImplementation == null) throw new System.Exception("Currently not in an implementation block!");
            return CurrImplementation.Type;
        }

    }

}