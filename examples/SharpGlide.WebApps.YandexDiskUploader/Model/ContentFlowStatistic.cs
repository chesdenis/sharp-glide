namespace SharpGlide.WebApps.YandexDiskUploader.Model
{
    public class ContentFlowStatistic
    {
        public double SpeedMbPerSec { get; set; }
        public double SpeedFilesPerSec { get; set; }
        public long TimeSpentSec { get; set; }
        public long FinishInSec { get; set; }
        public long FilesCount { get; set; }
        public long FoldersCount { get; set; }
        public long TotalSize { get; set; }
    }
}