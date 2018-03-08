using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Marker
{
    public class MarkdownSerializer<T>
    {
        private List<PropertyInfo> ContentProperties;
        private List<PropertyInfo> FrontmatterProperties;

        public MarkdownSerializer()
        {
            ContentProperties = new List<PropertyInfo>();
            FrontmatterProperties = new List<PropertyInfo>();
            var type = typeof(T);
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                if(Attribute.IsDefined(property, typeof(Frontmatter)))
                {
                    FrontmatterProperties.Add(property);
                }
                if(Attribute.IsDefined(property, typeof(Content)))
                {
                    ContentProperties.Add(property);
                }
            }
        }

        public void Serialize(T obj, Stream stream)
        {
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine(Markdown.FrontmatterDelimiter);
            foreach (var property in this.FrontmatterProperties)
            {
                writer.WriteLine("{0} : {1}", property.Name, property.GetValue(obj));
            }
            writer.WriteLine(Markdown.FrontmatterDelimiter);
            foreach (var property in this.ContentProperties)
            {
                writer.Write(property.GetValue(obj));
            }
            writer.Flush();
        }
    }
}