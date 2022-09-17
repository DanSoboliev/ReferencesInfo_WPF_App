using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Context.Entities {
    public class InfoContext {
        public static readonly Dictionary<string, EntityInfo> EntitiesInfo = 
            new Dictionary<string, EntityInfo> ();
    }
}
