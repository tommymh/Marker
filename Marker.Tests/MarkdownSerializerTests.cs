using System;
using System.Globalization;
using System.IO;
using System.Text;

using AutoFixture;
using Xunit;
using Xunit.Abstractions;

namespace Marker.Tests
{
    public class MarkdownSerializerTests
    {

        [Fact]
        public void SerializeTest()
        {
            using (MemoryStream outputStream = new MemoryStream())
            using (MemoryStream comparisonStream = new MemoryStream())
            {
                Fixture fixture = new Fixture();
                MarkdownSerializer markdownSerializer = new MarkdownSerializer();
                MockDoc mockDoc = fixture.Create<MockDoc>();
                TextWriter writer = new StreamWriter(comparisonStream);
                markdownSerializer.Serialize(mockDoc, outputStream);
                writer.WriteLine(Markdown.FrontmatterDelimiter);
                writer.WriteLine("{");
                writer.WriteLine("  \"{0}\": \"{1}\",", nameof(mockDoc.Title), mockDoc.Title);
                writer.WriteLine("  \"{0}\": \"{1}\",", nameof(mockDoc.Author), mockDoc.Author);
                writer.WriteLine("  \"{0}\": \"{1}\"", nameof(mockDoc.Date), mockDoc.Date.ToString("o", CultureInfo.InvariantCulture));
                writer.WriteLine("}");
                writer.WriteLine(Markdown.FrontmatterDelimiter);
                writer.Write(mockDoc.Content);
                writer.Flush();
                Assert.Equal(outputStream.ToArray(),comparisonStream.ToArray());
            }
        }
    }
}