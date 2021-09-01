using System.Collections.Generic;
using System.Linq;
using LLVMSharp;
using LLVMSharp.Interop;

namespace StraitJacket.Constructs {

    // A modifier.
    public enum Modifier {
        Public,
        ProtectedInternal,
        PrivateProtected,
        Private,
        Protected,
        Internal,
        Static,
        Inline,
        Async,
        Unsafe
    }

    // Function definition.
    public class Function {
        public string Name;
        public bool Static;
        public bool Inline;
        public bool Async;
        public bool Unsafe;
        public bool Extern;
        public bool Variadic;
        public Operators Operator;
        public List<Attribute> Attributes = new List<Attribute>();
        public List<VarParameter> Parameters;
        public VarType ReturnType;
        public CodeStatements Definition;
        public Scope Scope;
        
        public bool Compiled;
        public LLVMValueRef LLVMVal;
        public override string ToString() => (Extern || Name.Equals("main") || Attributes.Where(x => x.Name.Equals("NoMangle")).Count() > 0) ? Name : Mangler.MangleFunction(this);

        public void ResolveVariables() {
            if (Extern) return;
            Definition.ResolveVariables();
        }

        public void ResolveCalls() {
            if (Extern) return;
            Definition.ResolveCalls();
        }

        public void ResolveTypes() {
            if (Extern) return;
            Definition.ResolveTypes();
        }

        // TODO: NAME MANGLING AND MORE!!!
        public void Compile(LLVMModuleRef mod, LLVMBuilderRef builder) {
            if (Inline) Compiled = true;
            if (Compiled || Inline) return;
            int cnt = Parameters.Count;
            if (cnt > 0 && Parameters[cnt - 1].Type.Variadic) { cnt--; }
            LLVMTypeRef[] paramTypes = new LLVMTypeRef[cnt];
            for (int i = 0; i < cnt; i++) {
                paramTypes[i] = Parameters[i].Type.GetLLVMType();
            }
            LLVMVal = mod.AddFunction(Name, LLVMTypeRef.CreateFunction(ReturnType.GetLLVMType(), paramTypes, Variadic));
            if (!Extern) {
                var block = LLVMBasicBlockRef.AppendInContext(mod.Context, LLVMVal, "entry");
                builder.PositionAtEnd(block);
                Definition.Compile(mod, builder);
                if (ReturnType.Type == VarTypeEnum.Primitive && ReturnType.Primitive == Primitives.Void) {
                    builder.BuildRetVoid();
                }
            }
            Compiled = true;
        }

    }

}