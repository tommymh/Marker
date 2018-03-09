using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Marker
{
    public class MarkdownSerializer
    {
        private List<PropertyInfo> ContentProperties;
        private List<PropertyInfo> FrontmatterProperties;
        
        public MarkdownSerializer() { }        
        public void EnumerateProperties(Object obj) {
            ContentProperties = new List<PropertyInfo>();
            FrontmatterProperties = new List<PropertyInfo>();
            var type = obj.GetType();
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

        public void Serialize(Object obj, Stream stream)
        {
            this.EnumerateProperties(obj);
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