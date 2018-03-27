using System;
using System.Collections.Generic;
using System.IO;

namespace Marker {
    public interface IFrontmatterParser {
        Dictionary<string,object> Parse(string frontmatter);
    }
}