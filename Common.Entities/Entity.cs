using System;
using Common.Context.LineIndents;
using Common.Entities.Context;

namespace Common.Entities {
    [Serializable]
    public abstract class Entity
    {
        public int Id { get; set; }
        public abstract string ToMembersString();
        public sealed override string ToString() {
            return string.Format($"{Id,5} {ToMembersString()}");
        }
    }
}
