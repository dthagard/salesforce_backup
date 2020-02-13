using System;
using System.IO;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using SalesForceBackup.Interfaces;
using TinyIoC;

namespace SalesForceBackup.Uploaders
{
    /// <summary>
    /// Uploads files to AWS S3.
    /// </summary>
    public class S3Uploader : IUploader
    {
        private readonly IAppSettings _appSettings;
        private readonly IErrorHandler _errorHandler;

        /// <summary>
        /// Initializes a new S3Uploader.
        /// </summary>
        public S3Uploader()
        {
            _appSettings = TinyIoCContainer.Current.Resolve<IAppSettings>();
            _errorHandler = TinyIoCContainer.Current.Resolve<IErrorHandler>();
        }

        /// <summary>
        /// Uploads a file to S3.
        /// </summary>
        /// <param name="file">The full filename and path of the file to upload.</param>
        /// <param name="targetName">The file name that should be used for the upload</param>
        public void Upload(string file, String targetName)
        {
            IFormatProvider formatProvider = TinyIoCContainer.Current.Resolve<IFormatProvider>();
            try
            {
                var credentials = new BasicAWSCredentials(_appSettings.Get(AppSettingKeys.AwsAccessKey), _appSettings.Get(AppSettingKeys.AwsSecretKey));
                var region = RegionEndpoint.GetBySystemName(_appSettings.Get(AppSettingKeys.AwsRegion));
                using (var client = new AmazonS3Client(credentials, region))
                {
                    var request = new PutObjectRequest
                    {
                        BucketName = _appSettings.Get(AppSettingKeys.S3Bucket),
                        Key = String.Join("/", new[] { _appSettings.Get(AppSettingKeys.S3Folder), targetName }),
                        FilePath = file
                    };
                    Console.Write(string.Format(formatProvider, Properties.Resources.StatusUploadingS3, targetName));
                    client.PutObject(request);
                    Console.WriteLine("\u221A");
                }
            }
            catch (AmazonS3Exception e)
            {
                if (e.ErrorCode != null && ("InvalidAccessKeyId" == e.ErrorCode || "InvalidSecurity" == e.ErrorCode))
                {
                    _errorHandler.HandleError(e, (int)Enums.ExitCode.AwsCredentials, Properties.Resources.ConfigurationS3Credentials);
                }
                else
                {
                    _errorHandler.HandleError(e, (int)Enums.ExitCode.AwsS3Error, string.Format(formatProvider, Properties.Resources.StatusS3UploadFailed, e.Message));
                }
            } 
            finally
            {
                try
                {
                    if (File.Exists(file))
                        File.Delete(file);
                } catch(Exception ex)
                {
                    _errorHandler.HandleError(ex, (int)Enums.ExitCode.AwsS3Error, string.Format(formatProvider, Properties.Resources.StatusUnknownError, ex.Message));
                }
            }
        }
    }
}
