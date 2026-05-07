/* S!--CODE ATTRIBUTION
TITLE: Azure Blob Storage Client Library for .NET (Quickstart)
AUTHOR: Microsoft Documentation Team
DATE: 7 May 2026
VERSION: .NET SDK v12
AVAILABLE: https://learn.microsoft.com/en-us/azure/storage/blobs/storage-quickstart-blobs-dotnet
*/
/* S!--CODE ATTRIBUTION
TITLE: Azure SDK for .NET (GitHub Repository Samples)
AUTHOR: Azure SDK Contributors (GitHub)
DATE: 7 May 2026
VERSION: Azure.Storage.Blobs v12
AVAILABLE: https://github.com/Azure/azure-sdk-for-net/tree/main/sdk/storage/Azure.Storage.Blobs/samples
*/

/* S!--CODE ATTRIBUTION
TITLE: DotNet Tutorials: Azure Blob Storage with ASP.NET Core MVC
AUTHOR: DotNet Tutorials (YouTube Channel)
DATE: 7 May 2026
VERSION: Video Tutorial
AVAILABLE: https://www.youtube.com/watch?v=kYv_8E46T30
*/

using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

namespace Event_Ease2.Services
    {
        public class BlobService
        {
            private readonly BlobServiceClient _blobServiceClient;

        public BlobService(IConfiguration configuration)
        {
            var options = new BlobClientOptions(BlobClientOptions.ServiceVersion.V2024_11_04);
            _blobServiceClient = new BlobServiceClient(configuration.GetConnectionString("AzureStorage"), options);
        }
        public async Task<string> UploadFileAsync(IFormFile file, string containerName)
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                await containerClient.CreateIfNotExistsAsync();

                // Gives the image a unique name so they don't overwrite each other
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var blobClient = containerClient.GetBlobClient(fileName);

                using (var stream = file.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, true);
                }

                // This gives us back the URL link to save in the database
                return blobClient.Uri.ToString();
            }
        }
    }