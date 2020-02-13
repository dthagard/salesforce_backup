namespace SalesForceBackup.Interfaces
{

    /// <summary>
    /// Interface for uploaders to the backup storage.
    /// </summary>
    public interface IUploader
    {
        /// <summary>
        /// Uploads the specified file.
        /// </summary>
        /// <param name="file">The full name and path of the file to upload.</param>
        /// <param name="targetName">The file name that should be used for the upload</param>
        void Upload(string file, string targetName);
    }

}
