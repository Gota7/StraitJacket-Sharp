using System;
using System.Collections.Generic;
using LLVMSharp;
using LLVMSharp.Interop;

namespace StraitJacketLib.Constructs {

    public class Variable {
        public string Name;
        public Scope Scope;
        public VarType Type;
        public bool ScopeOverwriteable;
        public LLVMValueRef LLVMValue;
    }

}