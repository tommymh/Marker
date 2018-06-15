using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;

using Newtonsoft.Json;

namespace Marker {

    public class MarkdownDeserializer<T> {
        private PropertyInfo ContentProperty;
        private IFrontmatterParser FrontmatterParser;
        private List<PropertyInfo> FrontmatterProperties;

        public MarkdownDeserializer() {
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
                    ContentProperty = property;
                }
            }
            switch(Markdown.FrontmatterFormat) {
                case Frontmatter.Formats.YAML:
                this.FrontmatterParser = new YamlParser();
                break;
                case Frontmatter.Formats.JSON:
                this.FrontmatterParser = new JsonParser();
                break;
                case Frontmatter.Formats.TOML:
                this.FrontmatterParser = new TomlParser();
                break;
            }
        }

        public T Deserialize(Stream stream) {
            Object obj = Activator.CreateInstance(typeof(T));
            StringBuilder sb = new StringBuilder();
            StreamReader reader = new StreamReader(stream);
            string line;
            if((line = reader.ReadLine()) != null && line == Markdown.FrontmatterDelimiter) {
                ReadFrontmatter(ref obj, reader);
            } else {
                sb.Append(line);
                sb.Append(reader.ReadToEnd());
                ContentProperty.SetValue(obj, sb.ToString());
                return (T)obj;
            }
            ContentProperty.SetValue(obj, reader.ReadToEnd());
            return (T)obj;
        }

        public void ReadFrontmatter(ref Object obj, StreamReader reader) {
            string line;
            StringBuilder rawFrontmatter = new StringBuilder();
            while((line = reader.ReadLine())  != null) {
                if(line == Markdown.FrontmatterDelimiter) {
                    break;
                }
                rawFrontmatter.AppendLine(line);
            }
            var frontmatter = this.FrontmatterParser.Parse(rawFrontmatter.ToString());
            foreach (PropertyInfo property in this.FrontmatterProperties) {
                if(frontmatter[property.Name] != null)
                    SetProperty(ref obj, property, frontmatter[property.Name]);
            }
        }

        public void SetProperty(ref Object obj, PropertyInfo property, object value) {
            if( property.PropertyType == value.GetType()) {
                property.SetValue(obj, value);
            } else if(value.GetType() == typeof(string)) {
                TypeConverter tc = TypeDescriptor.GetConverter(property.PropertyType);
                property.SetValue(obj, tc.ConvertFromString((string)value));
            }
        }
    }
}