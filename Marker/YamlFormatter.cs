using System;
using System.IO;
using YamlDotNet.Serialization;

namespace Marker {
    public class YamlFormatter : IFrontmatterFormatter {
        public string Format(object obj) {
            var serializer = new Serializer();
            return serializer.Serialize(obj);
        }
    }
}