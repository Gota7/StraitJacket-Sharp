using System.Collections.Generic;
using System.Linq;
using StraitJacket.Constructs;

namespace StraitJacket.Builder {

    // Asylum program builder.
    public partial class ProgramBuilder {
        Implementation CurrImplementation = null;

        // Begin an implementation block.
        public void BeginImplementation(VarType type, VarType implements = null) {

        }

        // Define a function.
        public void BeginImplFunction(string name, VarType returnType, List<VarParameter> parameters) {
            parameters.Insert(0, new VarParameter() { Value = Variable(new VarTypePointer(ThisType()), "this") }); // Add this parameter.
            BeginFunction(name, returnType, parameters);
        }

        // Define a constructor.
        public void BeginImplConstructor(string name, List<VarParameter> parameters) {
            BeginFunction(name, ThisType(), parameters);
            VariableDefinition(null, Variable(ThisType(), "this")); // Define this.
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

        // End an implementation block.
        public void EndImplementation() {

        }

        // This type.
        public VarType ThisType() {
            if (CurrImplementation == null) throw new System.Exception("Currently not in an implementation block!");
            return CurrImplementation.Type;
        }

    }

}