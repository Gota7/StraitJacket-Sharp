using System;
using System.Collections.Generic;
using LLVMSharp;
using LLVMSharp.Interop;

namespace StraitJacketLib.Constructs {

    // Type implementation definition.
    public class Implementation : ICompileableUniversal {
        public VarType Type;
        public VarType InterfaceToImplement;
        public Dictionary<string, Function> Functions = new Dictionary<string, Function>();
        public Dictionary<Operator, Function> Operators = new Dictionary<Operator, Function>();
        public Dictionary<VarType, Function> ExplictCasts = new Dictionary<VarType, Function>();
        public Dictionary<VarType, Function> ImplicitCasts = new Dictionary<VarType, Function>();
        public FileContext FileContext;

        public FileContext GetFileContext() => FileContext;

        public void ResolveVariables() {}

        public void ResolveTypes() {}

        public void CompileDeclarations(LLVMModuleRef mod, LLVMBuilderRef builder, object param) {}

        public ReturnValue Compile(LLVMModuleRef mod, LLVMBuilderRef builder, object param) { return null; }

    }

}