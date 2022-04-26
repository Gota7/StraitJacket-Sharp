using System;
using System.Collections.Generic;
using LLVMSharp;
using LLVMSharp.Interop;

namespace WARD.Constructs {

    // Return value type.
    public enum ReturnValueType {
        Value,
        NestedValues,
        Void
    }

    // Any construct that can be compiled.
    public interface ICompileable {

        // Get file context.
        FileContext GetFileContext();

        // Convert variable names into references, which include function names.
        void ResolveVariables();

        // Resolve type names into references, also evaluate function overloads.
        void ResolveTypes();

        // Compile declarations.
        void CompileDeclarations(LLVMModuleRef mod, LLVMBuilderRef builder, object param);

        // Compile the item.
        LLVMValueRef Compile(LLVMModuleRef mod, LLVMBuilderRef builder, object param);

    }

    // ICompileable, but for universal statements.
    public interface ICompileableUniversal : ICompileable {}

}