using System.Collections.Generic;
using System.Linq;

namespace StraitJacketLib.Constructs {

    // Scope of variables, types, and functions. TODO: ALLOW SPLITTING NAMES BY PERIOD!!!
    public class Scope {
        public static Scope Root { get; internal set; } = null;
        public string Name;
        public Scope Parent;
        public Dictionary<string, Scope> Children = new Dictionary<string, Scope>();
        public static Dictionary<VarType, List<Implementation>> Implementations = new Dictionary<VarType, List<Implementation>>();
        private Dictionary<string, Dictionary<string, Function>> Functions = new Dictionary<string, Dictionary<string, Function>>();
        private Dictionary<string, Variable> Variables = new Dictionary<string, Variable>();
        private Dictionary<string, VarType> Types = new Dictionary<string, VarType>();
        private Dictionary<string, GenericTypeInfo> Generics = new Dictionary<string, GenericTypeInfo>();
        private static Stack<Function> CurrentFunction = new Stack<Function>();
        public static Function PeekCurrentFunction => CurrentFunction.Count == 0 ? null : CurrentFunction.Peek();

        public void AddFunction(string name, string mangled, Function v) {
            if (Functions.ContainsKey(name)) {
                if (Functions[name].ContainsKey(mangled)) {
                    throw new System.Exception("DUPLICATE FUNCTION!!!");
                } else {
                    Functions[name].Add(mangled, v);
                }
            } else {
                Functions.Add(name, new Dictionary<string, Function>());
                Functions[name].Add(mangled, v);
            }
        }

        public void AddVar(string name, Variable v) {
            if (Variables.ContainsKey(name)) {
                if (Variables[name].ScopeOverwriteable) {
                    Variables[name] = v;
                } else {
                    throw new System.Exception("DUPLICATE VARIABLE!!!");
                }
            } else {
                Variables.Add(name, v);
            }
        }

        public void AddType(string name, VarType v) {
            if (Types.ContainsKey(name)) {
                throw new System.Exception("DUPLICATE TYPE!!!");
            } else {
                Types.Add(name, v);
            }
        }

        public void AddGeneric(string name, GenericTypeInfo v) {
            if (Generics.ContainsKey(name)) {
                throw new System.Exception("DUPLICATE GENERIC!!!");
            } else {
                Generics.Add(name, v);
            }
        }

        public Function ResolveFunction(VariableOrFunction func) {
            if (func.Path.Equals("llvm")) {
                return AsyLLVM.Function;
            }
            if (Functions.ContainsKey(func.Path)) {
                return Functions[func.Path].Values.ElementAt(0);
            } else if (Parent != null) {
                return Parent.ResolveFunction(func);
            } else {
                throw new System.Exception("Function not resolved!");
            }
        }

        public Function ResolveFunctionVariable(VariableOrFunction v, List<VarType> paramTypes, VarType expectedReturnType = null) {

            // LLVM call.
            if (v.Path.Equals("llvm")) {
                return AsyLLVM.Function;
            }

            // Get possible functions.
            List<Function> funcs = new List<Function>();
            void AddFuncs(Scope s) {
                if (s.Functions.ContainsKey(v.Path)) {
                    funcs.AddRange(s.Functions[v.Path].Values);
                }
                if (s.Parent != null) AddFuncs(s.Parent);
            }
            AddFuncs(this);

            // Nothing found.
            if (funcs.Count == 0) {
                throw new System.Exception("Function overload not resolved!");
            }

            // Prune ineligible members.
            for (int i = funcs.Count - 1; i >= 0; i--) {
                if (paramTypes.Count < funcs[i].MinParameters() || paramTypes.Count > funcs[i].MaxParameters()) {
                    funcs.RemoveAt(i); // Wrong number of parameters passed.
                    continue;
                }
                for (int j = 0; j < paramTypes.Count; j++) {
                    int funcParamIndex = j;
                    if (funcParamIndex > funcs[i].Parameters.Count) funcParamIndex = funcs[i].Parameters.Count - 1;
                    if (!paramTypes[j].CanImplicitlyCastTo(funcs[i].Parameters[funcParamIndex].Value.Type)) {
                        funcs.RemoveAt(i);
                        break;
                    }
                }
            }

            // Only 1 matches.
            if (funcs.Count == 1) {
                return funcs[0];
            } else if (funcs.Count == 0) {
                throw new System.Exception("Function overload not resolved!");
            }

            // Get candidate functions from return type.
            List<Function> newFuncs = new List<Function>();
            if (expectedReturnType != null) {

                // First round, test for exact match.
                foreach (var o in funcs) {
                    if (o.ReturnType.Equals(expectedReturnType)) {
                        newFuncs.Add(o);
                    }
                }

                // Second round, test for implicit casts.
                if (newFuncs.Count == 0) {
                    foreach (var o in funcs) {
                        if (o.ReturnType.CanImplicitlyCastTo(expectedReturnType)) {
                            newFuncs.Add(o);
                        }
                    }
                }

            }
            else {
                foreach (var o in funcs) {
                    newFuncs.Add(o);
                }
            }

            // Only 1 matches.
            if (newFuncs.Count == 1) {
                return newFuncs[0];
            } else if (funcs.Count == 0) {
                throw new System.Exception("Function overload not resolved!");
            }

            // Get candidate functions from parameter.
            funcs = newFuncs;
            newFuncs = new List<Function>();

            // Check for exact matches.
            foreach (var o in funcs) {
                bool exactMatch = true;
                for (int i = 0; i < paramTypes.Count; i++) {
                    if (!paramTypes[i].Equals(o.Parameters[i].Value.Type)) {
                        exactMatch = false;
                        break;
                    }
                }
                if (exactMatch) newFuncs.Add(o);
            }

            // We know that all the functions can already be implicitly casted to, so if this didn't work then game over.
            if (newFuncs.Count == 1) {
                return newFuncs[0];
            }

            // Nothing was found, or too many were found.
            throw new System.Exception("Function overload not resolved!");

        }

        public List<Variable> ResolveVariable(VariableOrFunction v) {

            // Check if it is a parameter.
            List<Variable> ret = new List<Variable>();
            if (CurrentFunction.Count > 0) {
                var fn = CurrentFunction.Peek();
                 for (int i = 0; i < fn.Parameters.Count; i++) {
                    if (fn.Parameters[i].Value.Name.Equals(v.Path)) {
                        ret.Add(fn.Parameters[i].Value);
                    }
                }
            }

            // Then see if it is within this scope.
            if (Variables.ContainsKey(v.Path)) {
                ret.Add(Variables[v.Path]);
            }
            if (Functions.ContainsKey(v.Path)) {
                ret.AddRange(Functions[v.Path].Values);
            }
            if (ret.Count > 0) return ret;

            // Child scopes.
            if (Parent != null) {
                ret = Parent.ResolveVariable(v);
                if (ret.Count > 0) return ret;
            }

            // Hardcoded LLVM.
            if (v.Path.Equals("llvm")) {
                ret.Add(AsyLLVM.Function);
                return ret;
            }

            // Nothing found.
            throw new System.Exception("Variable not resolved!");

        }

        public VarType ResolveType(VariableOrFunction type) {
            if (Types.ContainsKey(type.Path)) {
                return Types[type.Path];
            } else if (Parent != null) {
                return Parent.ResolveType(type);
            } else {
                throw new System.Exception("Type not resolved!");
            }
        }

        // Push function so its parameters can be resolved.
        public static void PushFunction(Function f) {
            CurrentFunction.Push(f);
        }

        // Pop a function.
        public static void PopFunction() {
            CurrentFunction.Pop();
        }

        // String path.
        public override string ToString() {
            string ret = "";
            if (Parent != null && Parent.Name != "") ret = Parent.ToString() + ".";
            if (Name != "") ret += Name + ".";
            return ret;
        }

    }

}