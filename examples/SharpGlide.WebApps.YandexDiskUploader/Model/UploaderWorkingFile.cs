using SharpGlide.IO.Model;

namespace SharpGlide.WebApps.YandexDiskUploader.Model
{
    public class UploaderWorkingFile
    {
        private readonly FsEntryInfo _fileSystemSection;

        public UploaderWorkingFile(FsEntryInfo fileSystemSection)
        {
            _fileSystemSection = fileSystemSection;
        }
    }
}