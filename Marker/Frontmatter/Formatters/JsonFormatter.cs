using System;
using System.IO;
using Newtonsoft.Json;

namespace Marker {
    public class JsonFormatter : IFrontmatterFormatter {
        public string Format(object obj) {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }
    }
}