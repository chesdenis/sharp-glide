using System;
using System.IO;
using System.Linq;
using System.Web;

namespace SharpGlide.Cloud.Yandex.Extensions
{
    public static class CloudPathExtensions
    {
        public static string CalculateRelativePath(this string localPath, string relativeTo)
        {
            var localPathTrimmed = localPath.Trim().TrimStart('/', '\\').TrimEnd('/', '\\');
            var relativePathTrimmer = relativeTo.Trim().TrimStart('/', '\\').TrimEnd('/', '\\');

            var parts = localPathTrimmed
                .Split(relativePathTrimmer);
            if (parts.Length > 1)
            {
                var value = string.Join(string.Empty, parts.Skip(1).ToArray()).Replace('\\', '/');
                
                return $"/{value.TrimStart('/')}";
            }

            throw new Exception("Unable to calculate relative path");
        }

        public static string UrlEncode(this string path) => HttpUtility.UrlEncode(path);

        public static string UrlDecode(this string path) => HttpUtility.UrlDecode(path);
    }
}