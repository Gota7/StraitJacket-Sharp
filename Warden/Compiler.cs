using Antlr4.Runtime;
using Warden.AST;

namespace Warden {

    // Compilation flags.
    public class WardenCompilationFlags {
        // TODO!!!
    }

    // Compiler for C using the WARD API.
    public class WardenCompiler {
        Visitor visitor;

        // Initialize the ANTLR4 visitor.
        public WardenCompiler() {
            visitor = new Visitor();
        }

        // Visit a file.
        private void VisitFile(Stream s) {
            AntlrInputStream input = new AntlrInputStream(s);
            CLexer lexer = new CLexer(input);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            CParser parser = new CParser(commonTokenStream);
            var initContext = parser.compilationUnit();
            visitor.VisitCompilationUnit(initContext);
        }

        // Visit a file to be compiled.
        private void VisitFile(string s) {
            using (StreamReader fileStream = new StreamReader(s)) {
                VisitFile(fileStream.BaseStream);
            }
        }

        // Compile a file into an output.
        public void Compile(string inputFile, string outputFile, WardenCompilationFlags flags) {
            visitor.Builder.BeginFile(inputFile);
            VisitFile(inputFile);
            visitor.Builder.EndFile();
            var o = visitor.Builder.Compile();
            o[inputFile].WriteBitcodeToFile(outputFile);
        }

    }

}