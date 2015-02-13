using System;
using System.IO;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using SalesForceBackup.Interfaces;
using TinyIoC;

namespace SalesForceBackup
{
    /// <summary>
    /// Uploads backup files to Azure Blob storage.
    /// </summary>
    public class AzureBlobUploader : IUploader
    {

        private readonly IAppSettings _appSettings;
        private readonly IErrorHandler _errorHandler;

        public AzureBlobUploader()
        {
            _appSettings = TinyIoCContainer.Current.Resolve<IAppSettings>();
            _errorHandler = TinyIoCContainer.Current.Resolve<IErrorHandler>();
        }

        /// <summary>
        /// Uploads a file to Azure Blob.
        /// </summary>
        /// <param name="file">The full filename and path of the file to upload.</param>
        public void Upload(string file)
        {
            var blobEndpoint = new Uri(_appSettings.Get(AppSettingKeys.AzureBlobEndpoint));
            var accountName = _appSettings.Get(AppSettingKeys.AzureAccountName);
            var accountKey = _appSettings.Get(AppSettingKeys.AzureSharedKey);
            var containerName = _appSettings.Get(AppSettingKeys.AzureContainer);
            var blobName = String.Join("/", new[] {_appSettings.Get(AppSettingKeys.AzureFolder), Path.GetFileName(file)});

            try
            {
                var blobClient = new CloudBlobClient(blobEndpoint, new StorageCredentials(accountName, accountKey));
                var container = blobClient.GetContainerReference(containerName);
                container.CreateIfNotExists();
                var blob = container.GetBlockBlobReference(blobName);
                Console.WriteLine("Uploading {0} to Azure...", Path.GetFileName(file));
                blob.UploadFromFile(file, FileMode.OpenOrCreate);
            }
            catch (Exception e)
            {
                _errorHandler.HandleError(e, (int)Enums.ExitCode.AzureError, "There was an error uploading the file to Azure.");
            }
        }
    }
}
