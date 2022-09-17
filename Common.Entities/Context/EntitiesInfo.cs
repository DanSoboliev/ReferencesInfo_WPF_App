using System.Collections.Generic;

namespace Common.Entities.Context {
    public class EntitiesInfo {
        private static readonly Dictionary<string, string> names = new Dictionary<string, string>();
        public static void SetEntityName(string typeName, string name) {
            names[typeName] = name;
        }
        public static string GetEntityName(string typeName) {
            if (names.ContainsKey(typeName))
                return names[typeName];
            return typeName;
        }
    }
}
