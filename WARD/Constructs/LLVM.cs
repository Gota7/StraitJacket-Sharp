using System.Collections.Generic;
using LLVMSharp;
using LLVMSharp.Interop;

namespace WARD.Constructs {

    // LLVM function calls translate directly to LLVM assembly.
    public static class AsyLLVM {

        // The "normal" appearance of the LLVM assembly function.
        public static Function Function = new Function() {
            Name = "llvm",
            Static = true,
            Inline = true,
            Async = false,
            Unsafe = false,
            Extern = false,
            Variadic = true,
            Attributes = new List<Attribute>(),
            Parameters = new List<VarParameter>() {
                new VarParameter() { Value = new Variable() { Name = "instruction", Type = VarType.TypeConstString } },
                new VarParameter() { Value = new Variable() { Name = "args", Type = new VarTypeSimplePrimitive(SimplePrimitives.Object) { Variadic = true } } }
            },
            Type = new VarTypeFunction(
                VarType.TypeObject,
                new List<VarType>() {
                    VarType.TypeConstString,
                    new VarTypeSimplePrimitive(SimplePrimitives.Object) { Variadic = true }
                }
            ),
            ReturnType = VarType.TypeObject
        };

        // Since LLVM assembly calls are inline, they have to be defined for every call. Here we just translate the instructions.
        public static LLVMValueRef CompileCall(LLVMModuleRef mod, LLVMBuilderRef builder, List<Expression> args) {
            string instruction = (args[0] as ExpressionConstStringPtr).Str;
            LLVMValueRef v1 = args.Count > 1 ? args[1].Compile(mod, builder, null) : null;
            if (args.Count > 1 && args[1].LValue) v1 = builder.BuildLoad(v1, "SJ_LoadLLVMA1");
            LLVMValueRef v2 = args.Count > 2 ? args[2].Compile(mod, builder, null) : null;
            if (args.Count > 2 && args[2].LValue) v2 = builder.BuildLoad(v2, "SJ_LoadLLVMA2");
            switch (instruction) {
                case "add":
                    VerifyArgs(2);
                    return builder.BuildAdd(v1, v2);
                case "nswadd":
                    VerifyArgs(2);
                    return builder.BuildNSWAdd(v1, v2);
                case "nuwadd":
                    VerifyArgs(2);
                    return builder.BuildNUWAdd(v1, v2);
                case "sub":
                    VerifyArgs(2);
                    return builder.BuildSub(v1, v2);
                case "nswsub":
                    VerifyArgs(2);
                    return builder.BuildNSWSub(v1, v2);
                case "nuwsub":
                    VerifyArgs(2);
                    return builder.BuildNUWSub(v1, v2);
                case "mul":
                    VerifyArgs(2);
                    return builder.BuildMul(v1, v2);
                case "nswmul":
                    VerifyArgs(2);
                    return builder.BuildNSWMul(v1, v2);
                case "nuwmul":
                    VerifyArgs(2);
                    return builder.BuildNUWMul(v1, v2);
                case "udiv":
                    VerifyArgs(2);
                    return builder.BuildUDiv(v1, v2);
                case "sdiv":
                    VerifyArgs(2);
                    return builder.BuildSDiv(v1, v2);
                case "exactsdiv":
                    VerifyArgs(2);
                    return builder.BuildExactSDiv(v1, v2);
                case "urem":
                    VerifyArgs(2);
                    return builder.BuildURem(v1, v2);
                case "srem":
                    VerifyArgs(2);
                    return builder.BuildSRem(v1, v2);
                case "shl":
                    VerifyArgs(2);
                    return builder.BuildShl(v1, v2);
                case "lshr":
                    VerifyArgs(2);
                    return builder.BuildLShr(v1, v2);
                case "ashr":
                    VerifyArgs(2);
                    return builder.BuildAShr(v1, v2);
                case "and":
                    VerifyArgs(2);
                    return builder.BuildAnd(v1, v2);
                case "or":
                    VerifyArgs(2);
                    return builder.BuildOr(v1, v2);
                case "xor":
                    VerifyArgs(2);
                    return builder.BuildXor(v1, v2);
                case "not":
                    VerifyArgs(1);
                    return builder.BuildNot(v1);
                case "fadd":
                    VerifyArgs(2);
                    return builder.BuildFAdd(v1, v2);
                case "fsub":
                    VerifyArgs(2);
                    return builder.BuildFSub(v1, v2);
                case "fmul":
                    VerifyArgs(2);
                    return builder.BuildFMul(v1, v2);
                case "fdiv":
                    VerifyArgs(2);
                    return builder.BuildFDiv(v1, v2);
                case "frem":
                    VerifyArgs(2);
                    return builder.BuildFRem(v1, v2);
                case "fneg":
                    VerifyArgs(1);
                    return builder.BuildFNeg(v1);
                default:
                    throw new System.NotImplementedException();
            }
            void VerifyArgs(int num) {
                if (num != args.Count - 1) {
                    throw new System.Exception("LLVM assembly call with an invalid number of arguments!");
                }
            }
            throw new System.Exception("AHHHHH");
        }

    }

}