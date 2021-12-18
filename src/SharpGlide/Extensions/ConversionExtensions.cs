using System.IO;
using System.Xml.Serialization;

namespace SharpGlide.Extensions
{
    public static class ConversionExtensions
    {
        public static string AsXml<T>(this T input)
        {
            var serializer =
                new XmlSerializer(typeof(T));

            using var tw = new StringWriter();
            serializer.Serialize(tw, input);
            return tw.ToString();
        }
    }
}