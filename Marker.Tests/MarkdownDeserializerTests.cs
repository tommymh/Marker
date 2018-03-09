using System;
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
        public void DeserializeTest()
        {
            using (MemoryStream inputStream = new MemoryStream())
            {
                Fixture fixture = new Fixture();
                MarkdownDeserializer<MockDoc> ds = new MarkdownDeserializer<MockDoc>();
                MockDoc mockDoc = fixture.Create<MockDoc>();
                TextWriter writer = new StreamWriter(inputStream);
                writer.WriteLine(Markdown.FrontmatterDelimiter);
                writer.WriteLine("{0} : {1}", nameof(mockDoc.Title), mockDoc.Title);
                writer.WriteLine("{0} : {1}", nameof(mockDoc.Author), mockDoc.Author);
                writer.WriteLine("{0} : {1}", nameof(mockDoc.Date), mockDoc.Date);
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