using System;
using System.IO;

namespace Marker {
    public interface IFrontmatterFormatter {
        string Format(object obj);
    }
}