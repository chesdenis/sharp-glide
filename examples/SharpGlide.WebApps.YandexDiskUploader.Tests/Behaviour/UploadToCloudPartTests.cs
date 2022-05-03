using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Moq;
using SharpGlide.Cloud.Yandex.Writers.YandexDisc;
using SharpGlide.IO.Model;
using SharpGlide.IO.Readers;
using SharpGlide.IO.Tunnels;
using SharpGlide.Tunnels.Read.Abstractions;
using SharpGlide.Tunnels.Read.Model;
using SharpGlide.WebApps.YandexDiskUploader.Hubs;
using SharpGlide.WebApps.YandexDiskUploader.Parts;
using SharpGlide.WebApps.YandexDiskUploader.State;
using Xunit;

namespace SharpGlide.WebApps.YandexDiskUploader.Tests.Behaviour
{
    public class UploadToCloudPartTests
    {
        [Theory]
        [InlineData("/data", "/folderOnCloud")]
        [InlineData("data", "folderOnCloud")]
        [InlineData("//data", "//folderOnCloud")]
        [InlineData("//data/", "//folderOnCloud/")]
        [InlineData("//data///", "//folderOnCloud///")]
        public async Task ShouldCreateFoldersOnCloud(string localFolder, string cloudFolder)
        {
            // Arrange
            var storagePointer = new List<string>();
            storagePointer.Add("/data/testFolder1/subFolder1/testFileA.jpg");
            storagePointer.Add("/data/testFolder2/testFileB.jpg");
            storagePointer.Add("/data/testFolder3/testFileC.jpg");
            
            var mockStateRoot = new Mock<IStateRoot>();
            mockStateRoot.Setup(x => x.LocalFolder).Returns(localFolder);
            mockStateRoot.Setup(x => x.CloudFolder).Returns(cloudFolder);
            var mockHubContext = new Mock<IHubContext<RealtimeUpdatesHub>>();

            var mockFileSystemTunnel = new MockFileSystemTunnel(() => storagePointer);
            var mockFileSystemWalker = new FileSystemWalker(
                mockFileSystemTunnel.WalkSingleExpr.Compile(),
                mockFileSystemTunnel.WalkSingleAsyncExpr.Compile(),
                mockFileSystemTunnel.WalkPagedExpr.Compile(),
                mockFileSystemTunnel.WalkPagedAsyncExpr.Compile());

            var mockSingleFolderCreator = new Mock<ISingleFolderCreator>();
                
            var singleFileUploader = new Mock<ISingleFileUploader>();
            var sut = new UploadToCloudPart(
                mockStateRoot.Object,
                mockHubContext.Object,
                mockFileSystemWalker,
                singleFileUploader.Object,
                mockSingleFolderCreator.Object
            );

            // Act
            await sut.ProcessAsync(CancellationToken.None);
            
            // Assert
        }
        
        private class MockFileSystemTunnel : WalkTunnel<FsEntryInfo, FsEntryInfo>, IFileSystemWalkTunnel
        {
            private readonly Func<List<string>> _storagePointer;
            
            public MockFileSystemTunnel(Func<List<string>> storagePointer)
            {
                _storagePointer = storagePointer;
            }

            protected override async Task SingleWalkImpl(CancellationToken cancellationToken, FsEntryInfo request, Action<FsEntryInfo> callback)
            {
                throw new NotImplementedException();
            }

            protected override async Task SingleAsyncWalkImpl(CancellationToken cancellationToken, FsEntryInfo request, Func<CancellationToken, FsEntryInfo, Task> callback)
            {
                throw new NotImplementedException();
            }

            protected override async Task PagedWalkImpl(CancellationToken cancellationToken, PageInfo pageInfo, FsEntryInfo request, Action<IEnumerable<FsEntryInfo>> callback)
            {
                throw new NotImplementedException();
            }

            protected override async Task PagedAsyncWalkImpl(CancellationToken cancellationToken, PageInfo pageInfo, FsEntryInfo request, Func<CancellationToken, IEnumerable<FsEntryInfo>, Task> callback)
            {
                throw new NotImplementedException();
            }
        }
    }
}