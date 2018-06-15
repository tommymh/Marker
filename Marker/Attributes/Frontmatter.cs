using System;

namespace Marker
{
    public class Frontmatter : Attribute
    {
        public enum Formats { YAML, JSON, TOML }
    }
}