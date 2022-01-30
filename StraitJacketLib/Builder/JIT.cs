using System.Collections.Generic;
using StraitJacketLib.Constructs;
using LLVMSharp.Interop;
using System.Linq;

namespace StraitJacketLib.Builder {

    // Asylum program builder.
    public partial class ProgramBuilder {
        bool JITMode;
        bool JITInit;
        Dictionary<string, LLVMModuleRef> Mods;
        LLVMModuleRef JITMod;
        LLVMBuilderRef JITBuilder;
        LLVMExecutionEngineRef JITExe;

        // Initialize JIT.
        private void InitJIT() {
            LLVM.LinkInMCJIT();
            LLVM.InitializeNativeTarget();
            LLVM.InitializeNativeAsmPrinter();
            LLVM.InitializeNativeAsmParser();
            //JITMod.DataLayout = JITExe.TargetMachine.CreateTargetDataLayout().ToString();
            Mods = Compile(); // Must compile first in order to use in JIT!
            JITInit = true;
        }

        // Begin JIT Mode.
        public void BeginJITMode() {
            if (!JITInit) InitJIT();
            JITMod = LLVMModuleRef.CreateWithName("JIT");
            JITBuilder = LLVMBuilderRef.Create(JITMod.Context);
            LLVMPassManagerRef p = JITMod.CreateFunctionPassManager();
            JITExe = JITMod.CreateExecutionEngine();
            JITMode = true;
            p.AddBasicAliasAnalysisPass();
            p.AddPromoteMemoryToRegisterPass();
            p.AddInstructionCombiningPass();
            p.AddReassociatePass();
            p.AddGVNPass();
            p.AddCFGSimplificationPass();
            p.InitializeFunctionPassManager();
        }

        // End JIT Mode.
        public void EndJITMode() {
            JITMode = false;
        }

    }

}