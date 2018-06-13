using System;
using System.Collections.Generic;

using YamlDotNet.Serialization;

namespace Marker {
    public class YamlParser : IFrontmatterParser {
        public Dictionary<string,object> Parse(string frontmatter) {
            var deserializer = new Deserializer();
            return deserializer.Deserialize<Dictionary<string,object>>(frontmatter);
        }
    }
}