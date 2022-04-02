using WARD.Constructs;

namespace Warden.AST {

    // Result for storing visitor data.
    public class Result {
        public VarType Type; // Return type.
        public ICompileable CodeStatement; // General statement or list of statements.
        public Expression Expression; // General expression.
    }

}