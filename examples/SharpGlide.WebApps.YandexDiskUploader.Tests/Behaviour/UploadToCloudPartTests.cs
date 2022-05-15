using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.SignalR;
using Moq;
using SharpGlide.Cloud.Yandex.Extensions;
using SharpGlide.Cloud.Yandex.Model;
using SharpGlide.Cloud.Yandex.Tunnels.YandexDisk.Model;
using SharpGlide.Cloud.Yandex.Writers.YandexDisc;
using SharpGlide.IO.Model;
using SharpGlide.IO.Readers;
using SharpGlide.IO.Tunnels;
using SharpGlide.Processing;
using SharpGlide.Routing;
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
            var storagePointer = InitStorage();

            var mockStateRoot = ConfigureState(localFolder, cloudFolder);
            var mockHubContext = ConfigureHubContext();
            var mockFileSystemWalker = ConfigureFileSystemWalker(storagePointer);
            var createdFolders = ConfigureSingleFolderCreator(out var mockSingleFolderCreator);
            var singleFileUploader = new Mock<ISingleFileUploader>();
            var fileContentsWalker = new Mock<IFileContentWalker>();
            var speedMetric = new Mock<ISpeedMeasurePart>();
            
            var sut = new UploadToCloudPart(
                mockStateRoot.Object,
                mockHubContext.Object,
                mockFileSystemWalker,
                fileContentsWalker.Object,
                singleFileUploader.Object,
                mockSingleFolderCreator.Object
            );

            // Act
            var ct = CancellationToken.None;
            await sut.ProcessAsync(ct);

            // Assert
            var expectedFolderOnCloud = new[]
            {
                "/testFolder1/subFolder1",
                "/testFolder2",
                "/testFolder3"
            };
            foreach (var expectedFolder in expectedFolderOnCloud)
            {
                createdFolders.Single(w => w.CloudRelativePath.UrlDecode() == expectedFolder).Should().NotBeNull();
            }
        }
        
        [Theory]
        [InlineData("/data", "/folderOnCloud")]
        [InlineData("data", "folderOnCloud")]
        [InlineData("//data", "//folderOnCloud")]
        [InlineData("//data/", "//folderOnCloud/")]
        [InlineData("//data///", "//folderOnCloud///")]
        public async Task ShouldNotCreateFoldersOnCloud(string localFolder, string cloudFolder)
        {
            // Arrange
            var storagePointer = InitStorage();

            var mockStateRoot = ConfigureState(localFolder, cloudFolder);
            var mockHubContext = ConfigureHubContext();
            var fileSystemWalker = ConfigureFileSystemWalker(storagePointer);
            var createdFolders = ConfigureSingleFolderCreator(out var mockSingleFolderCreator);
            var singleFileUploader = new Mock<ISingleFileUploader>();
            var fileContentsWalker = new Mock<IFileContentWalker>();
            var speedMetric = new Mock<ISpeedMeasurePart>();

            var sut = new UploadToCloudPart(
                mockStateRoot.Object,
                mockHubContext.Object,
                fileSystemWalker,
                fileContentsWalker.Object,
                singleFileUploader.Object,
                mockSingleFolderCreator.Object
            );

            // Act
            var ct = CancellationToken.None;
            await sut.ProcessAsync(ct);

            // Assert
            var expectedFolderOnCloud = new[]
            {
                "/testFolder1/subFolder1",
                "/testFolder2",
                "/testFolder3"
            };
            foreach (var expectedFolder in expectedFolderOnCloud)
            {
                createdFolders.Single(w => w.CloudRelativePath.UrlDecode() == expectedFolder).Should().NotBeNull();
            }
        }

        private static List<string> InitStorage()
        {
            var storagePointer = new List<string>();
            storagePointer.Add("/data/testFolder1/subFolder1/testFileA.jpg");
            storagePointer.Add("/data/testFolder2/testFileB.jpg");
            storagePointer.Add("/data/testFolder3/testFileC.jpg");
            return storagePointer;
        }

        private static List<ICloudFolderInformation> ConfigureSingleFolderCreator(
            out Mock<ISingleFolderCreator> mockSingleFolderCreator)
        {
            var createdFolders = new List<ICloudFolderInformation>();
            mockSingleFolderCreator = new Mock<ISingleFolderCreator>();
            mockSingleFolderCreator.Setup(x => x.WriteSingle(
                    It.IsAny<IAuthorizeTokens>(),
                    It.IsAny<ICloudFolderInformation>(),
                    It.IsAny<IRoute>(),
                    It.IsAny<CancellationToken>()))
                .Returns<IAuthorizeTokens, ICloudFolderInformation, IRoute, CancellationToken>(
                    async (authorizationTokens, folder, route, cancellationToken) => { createdFolders.Add(folder); });
            return createdFolders;
        }

        private static FileSystemWalker ConfigureFileSystemWalker(List<string> storagePointer)
        {
            var mockFileSystemTunnel = new MockFileSystemTunnel(() => storagePointer);
            var mockFileSystemWalker = new FileSystemWalker(
                mockFileSystemTunnel.WalkSingleExpr.Compile(),
                mockFileSystemTunnel.WalkSingleAsyncExpr.Compile(),
                mockFileSystemTunnel.WalkPagedExpr.Compile(),
                mockFileSystemTunnel.WalkPagedAsyncExpr.Compile());
            return mockFileSystemWalker;
        }

        private static Mock<IStateRoot> ConfigureState(string localFolder, string cloudFolder)
        {
            var mockStateRoot = new Mock<IStateRoot>();
            mockStateRoot.Setup(x => x.LocalFolder).Returns(localFolder);
            mockStateRoot.Setup(x => x.CloudFolder).Returns(cloudFolder);
            return mockStateRoot;
        }

        private static Mock<IHubContext<RealtimeUpdatesHub>> ConfigureHubContext()
        {
            var mockHubContext = new Mock<IHubContext<RealtimeUpdatesHub>>();
            mockHubContext.Setup(x => x.Clients.All)
                .Returns(new Mock<IClientProxy>().Object);
            return mockHubContext;
        }

        private class MockFileSystemTunnel : WalkTunnel<FsEntryInfo, FsEntryInfo>, IFileSystemWalkTunnel
        {
            private readonly Func<List<string>> _storagePointer;

            public MockFileSystemTunnel(Func<List<string>> storagePointer)
            {
                _storagePointer = storagePointer;
            }

            protected override async Task SingleWalkImpl(CancellationToken cancellationToken, FsEntryInfo request,
                Action<FsEntryInfo> callback)
            {
                foreach (var item in _storagePointer())
                {
                    callback(new FsEntryInfo()
                    {
                        FullName = item
                    });
                }
            }

            protected override async Task SingleAsyncWalkImpl(CancellationToken cancellationToken, FsEntryInfo request,
                Func<CancellationToken, FsEntryInfo, Task> callback)
            {
                foreach (var item in _storagePointer())
                {
                    await callback(cancellationToken, new FsEntryInfo()
                    {
                        FullName = item
                    });
                }
            }

            protected override async Task PagedWalkImpl(CancellationToken cancellationToken, PageInfo pageInfo,
                FsEntryInfo request, Action<IEnumerable<FsEntryInfo>> callback)
            {
                callback(_storagePointer().Select(s => new FsEntryInfo()
                {
                    FullName = s
                }).ToList());
            }

            protected override async Task PagedAsyncWalkImpl(CancellationToken cancellationToken, PageInfo pageInfo,
                FsEntryInfo request, Func<CancellationToken, IEnumerable<FsEntryInfo>, Task> callback)
            {
                await callback(cancellationToken, _storagePointer().Select(s => new FsEntryInfo()
                {
                    FullName = s
                }).ToList());
            }
        }
    }
}