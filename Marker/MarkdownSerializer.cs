using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Marker
{
    public class MarkdownSerializer
    {
        private IFrontmatterFormatter FrontmatterFormatter;
        public MarkdownSerializer() {
            switch(Markdown.FrontmatterFormat) {
                case Frontmatter.Formats.YAML:
                break;
                case Frontmatter.Formats.JSON:
                this.FrontmatterFormatter = new JsonFormatter();
                break;
            } 
        }        
        public (string Frontmatter,string Content) SplitObject(Object obj) {
            IDictionary<string,object> frontmatterProperties = new Dictionary<string,object>();
            StringBuilder content = new StringBuilder();
            var type = obj.GetType();
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                if(Attribute.IsDefined(property, typeof(Frontmatter)))
                {
                    frontmatterProperties.Add(property.Name, property.GetValue(obj));
                }
                if(Attribute.IsDefined(property, typeof(Content)))
                {
                    content.Append(property.GetValue(obj));
                }
            }
            var frontmatter = this.FrontmatterFormatter.Format(frontmatterProperties);
            return (frontmatter,content.ToString());
        }
        public void Serialize(Object obj, Stream stream)
        {
            StreamWriter writer = new StreamWriter(stream);
            var markdownDocument = SplitObject(obj);
            writer.WriteLine(Markdown.FrontmatterDelimiter);
            writer.WriteLine(markdownDocument.Frontmatter);
            writer.WriteLine(Markdown.FrontmatterDelimiter);
            writer.Write(markdownDocument.Content);
            writer.Flush();
        }
    }
}