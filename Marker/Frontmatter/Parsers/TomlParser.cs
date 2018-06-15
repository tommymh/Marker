using System;
using System.Collections.Generic;

using Nett;

namespace Marker {
    public class TomlParser : IFrontmatterParser {
        public Dictionary<string,object> Parse(string frontmatter) {      
            return Toml.ReadString<Dictionary<string,object>>(frontmatter);
        }
    }
}