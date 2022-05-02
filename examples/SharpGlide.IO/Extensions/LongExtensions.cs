namespace SharpGlide.IO.Extensions
{
    public static class LongExtensions
    {
        public static string ToReadable(this long sizeInBytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            var len = sizeInBytes;
            var order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }

            return $"{len:0.##} {sizes[order]}";
        }
    }
}