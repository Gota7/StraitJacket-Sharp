using System.Collections.Generic;
using System.Linq;

namespace WARD.Constructs {

    // An entry of a structure.
    public class StructEntry {
        public Modifier Modifier;
        public Variable Var;
    }

    // TODO: IMPLEMENTED STUCTS SHOULD BE SEPARATE VARIABLES!!! THIS WILL CHANGE HOW IDX IS CALCULATED!!!
    // THIS_CALL ALSO HAS TO BE IMPLEMENTED!!!
    public class VarTypeStruct : VarTypeTuple {
        public Scope Scope;
        public Modifier Modifier;
        public string Name;
        public List<StructEntry> Entries = new List<StructEntry>();
        public List<VarTypeStruct> TypeImplements = new List<VarTypeStruct>();
        public new List<VarType> Members => GetMemberList(Entries, TypeImplements);

        // Create a new struct type.
        public VarTypeStruct(Scope scope, Modifier modifier, string name, List<StructEntry> entries, List<VarTypeStruct> implements) : base(GetMemberList(entries, implements)) {
            Scope = scope;
            Modifier = modifier;
            Name = name;
            Entries = entries;
            TypeImplements = implements;
            scope.AddType(name, this);
        }

        public static List<VarType> GetMemberList(List<StructEntry> entries, List<VarTypeStruct> implements) {
            List<VarType> ret = new List<VarType>();
            foreach (var i in implements) {
                ret.AddRange(GetMemberList(i.Entries, i.TypeImplements)); // Add base struct members.
            }
            foreach (var e in entries) {
                ret.Add(e.Var.Type); // Add current struct members.
            }
            return ret;
        }

        // Get a member's idx. If the name starts with # then the idx of an implemented struct is returned.
        public uint GetMemberIdx(string name) {
            return uint.MaxValue;
        }

        // Get the member from a name. If the name starts with # then return the implemented struct.
        public Variable GetMember(string name) {
            return null; // TODO!!!
        }

        // Get the idx of a member from a name.

        // This is a struct and not a true tuple.
        public override bool IsStruct => true;

    }

}