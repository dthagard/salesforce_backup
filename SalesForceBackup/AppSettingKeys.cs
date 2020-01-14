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
        public const string DataExportPage = "DataExportPage";
        public const string DownloadPage = "DownloadPage";
        public const string FilenamePattern = "FilenamePattern";
        public const string Host = "Host";
        public const string Password = "Password";
        public const string S3Bucket = "S3Bucket";
        public const string S3Folder = "S3Folder";
        public const string Scheme = "Scheme";
        public const string SecurityToken = "SecurityToken";
        public const string Uploader = "Uploader";
        public const string Username = "Username";
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
