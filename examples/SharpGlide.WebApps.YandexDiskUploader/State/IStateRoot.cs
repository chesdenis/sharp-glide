namespace SharpGlide.WebApps.YandexDiskUploader.State
{
    public interface IStateRoot
    {
        string LocalFolder { get; set; }
        string CloudFolder { get; set; }
        
        SecurityState SecuritySection { get; set; }

        bool Authenticated { get; }
    }
}