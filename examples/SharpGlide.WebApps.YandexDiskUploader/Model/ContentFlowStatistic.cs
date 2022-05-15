namespace SharpGlide.WebApps.YandexDiskUploader.Model
{
    public class ContentFlowStatistic
    {
        public double FilesProgress { get; set; }
        public double SizeProgress { get; set; }
        
        public double SpeedBytesPerSec { get; set; }
        public double SpeedFilesPerSec { get; set; }
        
        public double TimeSpentSec { get; set; }
        public double FinishInSec { get; set; }
        public long FilesCount { get; set; }
        public long FoldersCount { get; set; }
        public long TotalSize { get; set; }
    }
}