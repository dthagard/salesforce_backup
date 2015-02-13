namespace SalesForceBackup
{
    /// <summary>
    /// Contains any enumerations for the application.
    /// </summary>
    public class Enums
    {
        /// <summary>
        /// List of possible error codes for the application.
        /// </summary>
        public enum ExitCode
        {
            Normal = 0,
            Unknown = -1,
            ConfigurationError = 10,
            AwsCredentials = 20,
            AwsS3Error =21,
            AzureError = 30
        }
    }
}
