using System.Text;
using System.Text.Json;

namespace XDataFlow.Extensions
{
    public static class DataExtensions
    {
        public static byte[] AsBytes<T>(this T data) => Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data));
        public static T FromBytes<T>(this byte[] data) => JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(data));
    }
}