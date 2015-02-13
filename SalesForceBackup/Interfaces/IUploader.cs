using System;

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
        void Upload(String file);
    }
}
