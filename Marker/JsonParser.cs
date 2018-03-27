using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Marker {
    public class JsonParser : IFrontmatterParser {
        public Dictionary<string,object> Parse(string frontmatter) {
            return JsonConvert.DeserializeObject<Dictionary<string,object>>(frontmatter.ToString());
        }
    }
}