namespace Warden {

    // Main program.
    public static class Program {

        // Main method.
        public static void Main(string[] args) {

            // For now create default arguments.
            if (args.Length == 0) {
                args = new string[] { "Tests/HelloWorld.c", "Tests/obj/HelloWorld.bc" };
            }

            // Get flags, then compile.
            WardenCompiler c = new WardenCompiler();
            c.Compile(args[0], args[1], new WardenCompilationFlags());

        }

    }

}