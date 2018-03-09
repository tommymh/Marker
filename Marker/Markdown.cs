using System;
using System.IO;

namespace Marker
{
    public static class Markdown
    {
        public static string FrontmatterDelimiter { get; set; } = "------";
        //TODO: JSON support
        //public static Frontmatter.Format FrontmatterFormat { get; set;} = Frontmatter.Format.YAML;

        public static T Deserialize<T>(Stream stream) {
            MarkdownDeserializer<T> ds = new MarkdownDeserializer<T>();
            return ds.Deserialize(stream);
        }
        public static void Serialize<T>(T obj, Stream stream) {
            MarkdownSerializer<T> ms = new MarkdownSerializer<T>();
            ms.Serialize(obj, stream);
        }
    }
}
