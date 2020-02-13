using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using SalesForceBackup.Interfaces;

namespace SalesForceBackup
{
    /// <summary>
    /// Backs up the data from Salesforce.com
    /// </summary>
    public class Backup
    {
        #region Instance variables

        private IUploader _uploader;
        private readonly IDownloader _downloader;
        private readonly IErrorHandler _errorHandler;
        private readonly List<string> _filesToDelete = new List<string>();

        public IUploader Uploader { get => _uploader; set => _uploader = value; }

        #endregion // Instance variables

        /// <summary>
        /// Instantiates a new Backup object.
        /// </summary>
        public Backup(IDownloader downloader, IErrorHandler errorHandler)
        {
            _downloader = downloader;
            _errorHandler = errorHandler;
        }

        /// <summary>
        /// Begins a backup of the current SalesForce.com data.
        /// </summary>
        public void Run()
        {
            var files = _downloader.Download();
            for(int i=0; i<files.Length; i++)
            {
                string file = files[i];
                _uploader.Upload(file, FormatFileName(Path.GetExtension(file), i));
            }
        }

        /// <summary>
        /// Formats the name of the file.
        /// </summary>
        /// <param name="extension">The extension to apply to the filename.</param>
        /// <param name="i">The revision number to apply to the filename.</param>
        /// <returns>The formatted filename.</returns>
        /// <remarks>If the revision is less than or equal to 0, then no revision indicator will be applied.</remarks>
        private static string FormatFileName(string extension, int i)
        {
            string dateTime = string.Format(CultureInfo.InvariantCulture, "{0}-{1}-{2}_{3}-{4}",
                DateTime.UtcNow.Year.ToString("D4", CultureInfo.InvariantCulture),
                DateTime.UtcNow.Month.ToString("D2", CultureInfo.InvariantCulture),
                DateTime.UtcNow.Day.ToString("D2", CultureInfo.InvariantCulture),
                DateTime.UtcNow.Hour.ToString("D2", CultureInfo.InvariantCulture),
                DateTime.UtcNow.Minute.ToString("D2", CultureInfo.InvariantCulture));
            return string.Format(CultureInfo.InvariantCulture, "{0}-{1}{2}", dateTime, i, extension);
        }

    }

}
