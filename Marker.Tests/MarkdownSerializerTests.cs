using System;
using System.IO;
using System.Text;

using AutoFixture;
using Xunit;
using Xunit.Abstractions;

namespace Marker.Tests
{
    public class MarkdownSerializerTests
    {
        private readonly ITestOutputHelper output;

        public MarkdownSerializerTests(ITestOutputHelper output) {
            this.output = output;
        }

        [Fact]
        public void SerializeTest()
        {
            using (MemoryStream outputStream = new MemoryStream())
            using (MemoryStream assertionStream = new MemoryStream())
            {
                Fixture fixture = new Fixture();
                MarkdownSerializer markdownSerializer = new MarkdownSerializer();
                Document mockDoc = fixture.Create<Document>();
                TextWriter writer = new StreamWriter(assertionStream);
                markdownSerializer.Serialize(mockDoc, outputStream);
                writer.WriteLine(Markdown.FrontmatterDelimiter);
                writer.WriteLine("{0} : {1}", nameof(mockDoc.Title), mockDoc.Title);
                writer.WriteLine("{0} : {1}", nameof(mockDoc.Author), mockDoc.Author);
                writer.WriteLine("{0} : {1}", nameof(mockDoc.Date), mockDoc.Date);
                writer.WriteLine(Markdown.FrontmatterDelimiter);
                writer.Write(mockDoc.Content);
                writer.Flush();
                Assert.Equal(outputStream.ToArray(),assertionStream.ToArray());
            }
        }

        public class Document {
            [Frontmatter]
            public string Title { get; set; }
            [Frontmatter]
            public string Author { get; set; }
            [Frontmatter]
            public DateTime Date { get; set; }
            [Content]
            public String Content { get; set; }
        }
    }
}
