using SharpGlide.Cloud.Yandex.Model;
using SharpGlide.Tunnels.Write.Interfaces;

namespace SharpGlide.Cloud.Yandex.Tunnels.YandexDisk
{
    public interface IDiskFileUploadTunnel : 
        ISingleWriteTunnel<IDiskFileUploadTunnel.IFileInformation, AuthorizeTokens>,
        ICollectionWriteTunnel<IDiskFileUploadTunnel.IFileInformation, AuthorizeTokens>
    {
        public interface IFileInformation
        {
            public string UploadUri { get; set; }
            public string StatusCode { get; set; }
            public string Reason { get; set; }
            public string FullName { get; set; }
            public string Name { get; set; }
            public long Size { get; set; }
        }
    }
}