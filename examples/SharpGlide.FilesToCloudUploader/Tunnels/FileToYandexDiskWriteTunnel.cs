using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Logging;
using SharpGlide.Routing;
using SharpGlide.Tunnels.Write.Abstractions;
using SharpGlide.Tunnels.Write.Interfaces;

namespace SharpGlide.FilesToCloudUploader.Tunnels
{
    // public class FileToYandexDiskWriteTunnel : WriteTunnel<FileToYandexDiskWriteTunnel.FileMetadata>
    // {
    //     private readonly ILogger<FileToYandexDiskWriteTunnel> _logger;
    //     private readonly HttpClient _httpClient;
    //     private readonly ApiSettings _apiSettings;
    //
    //     public int MaxDegreeOfParallelism { get; set; } = 6;
    //
    //     public FileToYandexDiskWriteTunnel(
    //         ILogger<FileToYandexDiskWriteTunnel> logger,
    //         HttpClient httpClient,
    //         ApiSettings apiSettings)
    //     {
    //         _logger = logger;
    //         _httpClient = httpClient;
    //         _apiSettings = apiSettings;
    //     }
    //
    //     public class ApiSettings
    //     {
    //         public string OAuthParameterValue { get; set; }
    //         public string ScheduleUploadEndpoint { get; set; }
    //     }
    //
    //     public class ApiResponse
    //     {
    //         public string href { get; set; }
    //         public string method { get; set; }
    //         public string templated { get; set; }
    //     }
    //
    //     public class FileMetadata
    //     {
    //         public string UploadUri { get; set; }
    //         public string StatusCode { get; set; }
    //         public string Reason { get; set; }
    //         public string FullName { get; set; }
    //         public string Name { get; set; }
    //         public long Size { get; set; }
    //     }
    //
    //     protected override async Task WriteSingleImpl(FileMetadata data, IRoute route,
    //         CancellationToken cancellationToken)
    //     {
    //         await EnrichWithUploadUri(data, cancellationToken);
    //         await Upload(data, cancellationToken);
    //     }
    //
    //     protected override async Task<FileMetadata> WriteAndReturnSingleImpl(FileMetadata data, IRoute route,
    //         CancellationToken cancellationToken)
    //     {
    //         await EnrichWithUploadUri(data, cancellationToken);
    //         await Upload(data, cancellationToken);
    //
    //         return data;
    //     }
    //
    //     protected override async Task WriteRangeImpl(IEnumerable<FileMetadata> dataRange, IRoute route,
    //         CancellationToken cancellationToken)
    //     {
    //         var dataRangeAsArray = dataRange as FileMetadata[] ?? dataRange.ToArray();
    //         
    //         await EnrichWithUploadUri(dataRangeAsArray, cancellationToken);
    //         await Upload(dataRangeAsArray, cancellationToken);
    //     }
    //
    //     protected override async Task<IEnumerable<FileMetadata>> WriteAndReturnRangeImpl(
    //         IEnumerable<FileMetadata> dataRange,
    //         IRoute route, CancellationToken cancellationToken)
    //     {
    //         var dataRangeAsArray = dataRange as FileMetadata[] ?? dataRange.ToArray();
    //         
    //         await EnrichWithUploadUri(dataRangeAsArray, cancellationToken);
    //         await Upload(dataRangeAsArray, cancellationToken);
    //
    //         return dataRangeAsArray;
    //     }
    //
    //     private Task EnrichWithUploadUri(IEnumerable<FileMetadata> collection,
    //         CancellationToken cancellationToken)
    //     {
    //         _httpClient.DefaultRequestHeaders.Authorization =
    //             new AuthenticationHeaderValue("OAuth", _apiSettings.OAuthParameterValue);
    //
    //         var collectionAsArray = collection as FileMetadata[] ?? collection.ToArray();
    //
    //         Parallel.For(0, collectionAsArray.Length - 1, new ParallelOptions
    //             {
    //                 CancellationToken = cancellationToken,
    //                 MaxDegreeOfParallelism = MaxDegreeOfParallelism
    //             }, index =>
    //             {
    //                 var file = collectionAsArray[index];
    //                 EnrichWithUploadUri(file, cancellationToken).Wait(cancellationToken);
    //             }
    //         );
    //         
    //         return Task.CompletedTask;
    //     }
    //
    //     private async Task EnrichWithUploadUri(FileMetadata fileMetadata, CancellationToken cancellationToken)
    //     {
    //         var uploadUrl = HttpUtility.UrlEncode(fileMetadata.FullName);
    //         var parameters = new Dictionary<string, string> { { "path", uploadUrl } };
    //
    //         var response = await _httpClient.GetAsync(
    //             _apiSettings.ScheduleUploadEndpoint + "?" +
    //             string.Join("&",
    //                 parameters.Select(s => $"{s.Key}={s.Value}").ToArray()), cancellationToken);
    //
    //         if (response.StatusCode == HttpStatusCode.OK)
    //         {
    //             var responseBody = await response.Content.ReadFromJsonAsync<ApiResponse>(cancellationToken: cancellationToken);
    //
    //             if (responseBody == null)
    //             {
    //                 throw new ArgumentNullException(nameof(responseBody),
    //                     $"Unable to deserialize response from api when requesting upload uri for {fileMetadata.FullName}");
    //             }
    //
    //             fileMetadata.UploadUri = responseBody.href;
    //         }
    //         else
    //         {
    //             throw new ArgumentOutOfRangeException(nameof(response),
    //                 $"Status code is not success when requesting file upload for {fileMetadata.FullName}");
    //         }
    //     }
    //
    //     private Task Upload(IEnumerable<FileMetadata> collection, CancellationToken cancellationToken)
    //     {
    //         var collectionAsArray = collection as FileMetadata[] ?? collection.ToArray();
    //
    //         Parallel.For(0, collectionAsArray.Length - 1, new ParallelOptions
    //             {
    //                 CancellationToken = cancellationToken,
    //                 MaxDegreeOfParallelism = MaxDegreeOfParallelism
    //             }, index =>
    //             {
    //                 var file = collectionAsArray[index];
    //                 Upload(file, cancellationToken).Wait(cancellationToken);
    //             }
    //         );
    //         
    //         return Task.CompletedTask;
    //     }
    //     private async Task Upload(FileMetadata fileMetadata, CancellationToken cancellationToken)
    //     {
    //         var streamContent = new StreamContent(File.OpenRead(fileMetadata.FullName));
    //         if (fileMetadata.UploadUri != null)
    //         {
    //             var response = await _httpClient.PutAsync(fileMetadata.UploadUri, streamContent, cancellationToken);
    //
    //             if (response.StatusCode != HttpStatusCode.OK)
    //             {
    //                 throw new ArgumentOutOfRangeException(nameof(response),
    //                     $"Status code is not success when uploading to cloud for {fileMetadata.FullName}. Reason is {response.ReasonPhrase}");
    //             }
    //         }
    //         else
    //         {
    //             throw new ArgumentNullException(nameof(fileMetadata.UploadUri),
    //                 $"Is empty for {fileMetadata.FullName}");
    //         }
    //     }
    // }
}