using System;
using System.Collections.Generic;
using System.Linq;
using LLVMSharp.Interop;

namespace StraitJacketLib.Constructs {

    // An entry of a structure.
    public class StructEntry {
        public Modifier Modifier;
        public Variable Var;
    }

    // A custom structure.
    public class VarTypeStruct : VarType {
        public Scope Scope;
        public Modifier Modifier;
        public string Name;
        public List<VarTypeStruct> TypeImplements = new List<VarTypeStruct>();
        public List<StructEntry> Entries = new List<StructEntry>();

        public VarTypeStruct(Scope scope, Modifier modifier, string name, List<StructEntry> entries, List<VarTypeStruct> implements) {
            Scope = scope;
            Modifier = modifier;
            Name = name;
            Entries = entries;
            TypeImplements = implements;
            Type = VarTypeEnum.Tuple;
        }

        // Get the type of a member. TODO: BASE TYPES!!!
        public VarType GetMemberType(string varName, string currBase = null) {
            string[] parts = varName.Split('.');
            if (parts[0].Equals("this")) parts[0] = parts[1];
            foreach (var e in Entries) {
                if (parts[0].Equals(e.Var.Name)) {
                    return e.Var.Type;
                }
            }
            var func = GetImplFunction(varName);
            if (func != null) return func.Type;
            throw new Exception("Member " + varName + " is not contained in this struct!");
        }

        public Function GetImplFunction(string name) {
            Scope root = Scope;
            while (root.Parent != null) {
                root = root.Parent;
            }
            string mangled = Mangler.MangleType(this);
            if (root.Children.ContainsKey(mangled)) {
                Scope typeScope = root.Children[mangled];
                return typeScope.ResolveFunction(new VariableOrFunction() { Scope = typeScope, Path = name });
            }
            return null;
        }

        // Calculate an Idx to a name. TODO: BASE TYPES!!!
        public uint CalcIdx(string varName, string currBase = null) {
            uint idx = 0;
            string[] parts = varName.Split('.');
            if (parts[0].Equals("this")) parts[0] = parts[1];
            for (int i = 0; i < TypeImplements.Count; i++) {
                //if (parts[0].) TODO!!!
                idx += TypeImplements[i].NumIdxs();
            }
            foreach (var e in Entries) {
                if (parts[0].Equals(e.Var.Name)) {
                    return idx;
                }
                idx++;
            }
            throw new Exception("Member " + varName + " is not contained in this struct!");
        }

        // Get the total number of Idxs.
        public uint NumIdxs() {
            uint idx = 0;
            for (int i = 0; i < TypeImplements.Count; i++) {
                idx += TypeImplements[i].NumIdxs();
            }
            foreach (var e in Entries) {
                idx++;
            }
            return idx;
        }

        protected override LLVMTypeRef LLVMType() {
            List<LLVMTypeRef> members = new List<LLVMTypeRef>();
            foreach (var i in TypeImplements) {
                foreach (var e in i.Entries) {
                    members.Add(e.Var.Type.GetLLVMType());
                }
            }
            foreach (var m in Entries) {
                members.Add(m.Var.Type.GetLLVMType());
            }
            return LLVMTypeRef.CreateStruct(members.ToArray(), false);
        }

        protected override string Mangled() => Mangler.MangleScope(Scope) + Name.Length + Name + "E";

        public override bool Equals(object obj) {
            // TODO!!!
            throw new System.NotImplementedException();
        }

        public override int GetHashCode() {
            // TODO!!!
            throw new System.NotImplementedException();
        }

        public override string ToString() {
            return Name;
        }

    }

}