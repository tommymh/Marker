using System;

namespace Marker
{
    public class Frontmatter : Attribute
    {
        public enum Format { JSON, YAML }
    }
}