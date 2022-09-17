using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Context.Entities {
    public class EntityInfo {
        //public string TypeName { get; set; }
        public string Name { get; set; }
        public string ListHeader { get; set; }
        public string Description { get; set; }

        public EntityInfo(string name, string listHeader) {
            Name = name;
            ListHeader = listHeader;
        }
    }
}
