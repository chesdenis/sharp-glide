using System.IO;
using System.Linq;
using System.Web;

namespace SharpGlide.Cloud.Yandex.Extensions
{
    public static class CloudPathExtensions
    {
        public static string CalculateRelativePath(this string localPath, string relativeTo)
        {
            return localPath.Substring(relativeTo.Length);
        }

        public static string ToCloudPath(this string localPath)
        {
            if (localPath.Contains('/'))
            {
                return HttpUtility.UrlEncode(localPath);
            }
            else if (localPath.Contains('\\'))
            {
                return HttpUtility.UrlEncode(localPath.Replace('\\', '/'));
            }

            return HttpUtility.UrlEncode($"/{localPath.TrimStart('/')}");
        }
    }
}