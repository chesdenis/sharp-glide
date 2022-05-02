namespace SharpGlide.WebApps.YandexDiskUploader.State
{
    public interface IStateRoot
    {
        string WorkingFolder { get; set; }
        
        SecurityState SecuritySection { get; set; }

        bool Authenticated { get; }
    }
}