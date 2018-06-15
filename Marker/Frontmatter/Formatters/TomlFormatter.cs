using System;
using System.IO;
using Nett;

namespace Marker {
    public class TomlFormatter : IFrontmatterFormatter {
        public string Format(object obj) {
            return Toml.WriteString<object>(obj);
        }
    }
}