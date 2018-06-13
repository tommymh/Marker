using System;
using System.Globalization;
using System.IO;
using System.Text;

using AutoFixture;
using Xunit;
using Xunit.Abstractions;

namespace Marker.Tests
{
    public class MarkdownDeserializerTests
    {

        [Fact]
        public void JsonDeserializeTest()
        {
            using (MemoryStream inputStream = new MemoryStream())
            {
                Fixture fixture = new Fixture();
                Markdown.FrontmatterFormat = Frontmatter.Formats.JSON;
                MarkdownDeserializer<MockDoc> ds = new MarkdownDeserializer<MockDoc>();
                MockDoc mockDoc = fixture.Create<MockDoc>();
                StreamWriter writer = new StreamWriter(inputStream);
                writer.WriteLine(Markdown.FrontmatterDelimiter);
                writer.WriteLine("{");
                writer.WriteLine("  \"{0}\": \"{1}\",", nameof(mockDoc.Title), mockDoc.Title);
                writer.WriteLine("  \"{0}\": \"{1}\",", nameof(mockDoc.Author), mockDoc.Author);
                writer.WriteLine("  \"{0}\": \"{1}\"", nameof(mockDoc.Date), mockDoc.Date.ToString("o", CultureInfo.InvariantCulture));
                writer.WriteLine("}");
                writer.WriteLine(Markdown.FrontmatterDelimiter);
                writer.Write(mockDoc.Content);
                writer.Flush();
                inputStream.Seek(0, SeekOrigin.Begin);
                var newDoc = ds.Deserialize(inputStream);
                Assert.Equal( new { mockDoc.Author, mockDoc.Title, mockDoc.Date.Date, mockDoc.Content },
                              new { newDoc.Author, newDoc.Title, newDoc.Date.Date, newDoc.Content });
            }
        }
        
        [Fact]
        public void YamlDeserializeTest()
        {
            using (MemoryStream inputStream = new MemoryStream())
            {
                Fixture fixture = new Fixture();
                Markdown.FrontmatterFormat = Frontmatter.Formats.YAML;
                MarkdownDeserializer<MockDoc> ds = new MarkdownDeserializer<MockDoc>();
                MockDoc mockDoc = fixture.Create<MockDoc>();
                StreamWriter writer = new StreamWriter(inputStream);
                writer.WriteLine(Markdown.FrontmatterDelimiter);
                writer.WriteLine("{0}: {1}", nameof(mockDoc.Title), mockDoc.Title);
                writer.WriteLine("{0}: {1}", nameof(mockDoc.Author), mockDoc.Author);
                writer.WriteLine("{0}: {1}", nameof(mockDoc.Date), mockDoc.Date.ToString("o", CultureInfo.InvariantCulture));
                writer.Write("\r\n");
                writer.WriteLine(Markdown.FrontmatterDelimiter);
                writer.Write(mockDoc.Content);
                writer.Flush();
                inputStream.Seek(0, SeekOrigin.Begin);
                var newDoc = ds.Deserialize(inputStream);
                Assert.Equal( new { mockDoc.Author, mockDoc.Title, mockDoc.Date.Date, mockDoc.Content },
                              new { newDoc.Author, newDoc.Title, newDoc.Date.Date, newDoc.Content });
            }
        }

    }
}