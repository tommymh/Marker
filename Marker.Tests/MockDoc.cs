using System;

namespace Marker.Tests {
        public class MockDoc {
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