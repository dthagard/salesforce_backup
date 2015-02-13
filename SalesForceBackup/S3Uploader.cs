using System;
using System.IO;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using SalesForceBackup.Interfaces;
using TinyIoC;

namespace SalesForceBackup
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
        public void Upload(string file)
        {
            var filename = Path.GetFileName(file);
            try
            {
                var credentials = new BasicAWSCredentials(_appSettings.Get(AppSettingKeys.AwsAccessKey), _appSettings.Get(AppSettingKeys.AwsSecretKey));
                var region = RegionEndpoint.GetBySystemName(_appSettings.Get(AppSettingKeys.AwsRegion));
                using (var client = new AmazonS3Client(credentials, region))
                {
                    var request = new PutObjectRequest
                    {
                        BucketName = _appSettings.Get(AppSettingKeys.S3Bucket),
                        Key = String.Join("/", new[] { _appSettings.Get(AppSettingKeys.S3Folder), filename }),
                        FilePath = file
                    };
                    Console.WriteLine("Uploading {0} to AWS S3...", filename);
                    client.PutObject(request);
                }
            }
            catch (AmazonS3Exception e)
            {
                if (e.ErrorCode != null && (e.ErrorCode.Equals("InvalidAccessKeyId") || e.ErrorCode.Equals("InvalidSecurity")))
                {
                    _errorHandler.HandleError(e, (int)Enums.ExitCode.AwsCredentials, "Check the provided AWS Credentials");
                }
                else
                {
                    _errorHandler.HandleError(e, (int)Enums.ExitCode.AwsS3Error, String.Format("Error occurred. Message:'{0}' when writing an object", e.Message));
                }
            }
        }
    }
}
