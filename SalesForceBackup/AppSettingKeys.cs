using Amazon.Runtime;

namespace SalesForceBackup
{
    /// <summary>
    /// Contains the keys that are defined in the App.Config.
    /// </summary>
    internal class AppSettingKeys
    {
        public const string AwsAccessKey = "AWSAccessKey";
        public const string AwsRegion = "AWSRegion";
        public const string AwsSecretKey = "AWSSecretKey";
        public const string AzureAccountName = "AzureAccountName";
        public const string AzureBlobEndpoint = "AzureBlobEndpoint";
        public const string AzureContainer = "AzureContainer";
        public const string AzureFolder = "AzureFolder";
        public const string AzureSharedKey = "AzureSharedKey";
        public const string DataExportPage = "dataExportPage";
        public const string DownloadPage = "downloadPage";
        public const string FilenamePattern = "filenamePattern";
        public const string Host = "host";
        public const string Password = "password";
        public const string OrganizationId = "organizationId";
        public const string S3Bucket = "S3Bucket";
        public const string S3Folder = "S3Folder";
        public const string Scheme = "scheme";
        public const string Uploader = "uploader";
        public const string Username = "username";
    }

    /// <summary>
    /// Contains all of the possible uploader values in the App.config.
    /// </summary>
    internal class Uploaders
    {
        public const string Aws = "AWS";
        public const string Azure = "Azure";
    }
}
