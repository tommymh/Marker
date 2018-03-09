using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;

namespace Marker {

    public class MarkdownDeserializer<T> {
        private PropertyInfo ContentProperty;
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
        }

        public Object Deserialize(Stream stream) {
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
                return obj;
            }
            ContentProperty.SetValue(obj, reader.ReadToEnd());
            return obj;
        }

        public void ReadFrontmatter(ref Object obj, StreamReader reader) {
            string line;
            while((line = reader.ReadLine())  != null) {
                if(line == Markdown.FrontmatterDelimiter) {
                    return;
                }
                int index = line.IndexOf(" : ");
                string propertyName = line.Substring(0, index);
                string propertyValue = line.Substring(index+3);
                foreach (PropertyInfo property in FrontmatterProperties) {
                    if(property.Name == propertyName) {
                        SetProperty(ref obj, property, propertyValue);
                    }
                }
            }
        }

        public void SetProperty(ref Object obj, PropertyInfo property, string value) {
            if( property.PropertyType == value.GetType()) {
                property.SetValue(obj, value);
            } else {
                TypeConverter tc = TypeDescriptor.GetConverter(property.PropertyType);
                property.SetValue(obj, tc.ConvertFromString(value));
            }
        }
    }
}
