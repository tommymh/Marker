using System;
using System.IO;

namespace Marker
{
    public static class Markdown
    {
        public static string FrontmatterDelimiter { get; set; } = "------";

        public static T Deserialize<T>(Stream stream) {
            MarkdownDeserializer<T> ds = new MarkdownDeserializer<T>();
            return ds.Deserialize(stream);
        }

        public static void Serialize(Object obj, Stream stream) {
            MarkdownSerializer ms = new MarkdownSerializer();
            ms.Serialize(obj, stream);
        }
    }
}
