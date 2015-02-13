using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using SalesForceBackup.Interfaces;

namespace SalesForceBackup
{
    /// <summary>
    /// Backs up the data from SalesForce.com
    /// </summary>
    public class Backup
    {
        #region Instance variables

        private readonly IUploader _uploader;
        private readonly IDownloader _downloader;
        private readonly IErrorHandler _errorHandler;
        private readonly List<string> _filesToDelete = new List<string>();

        #endregion // Instance variables

        /// <summary>
        /// Instantiates a new Backup object.
        /// </summary>
        public Backup(IUploader uploader, IDownloader downloader, IErrorHandler errorHandler)
        {
            _uploader = uploader;
            _downloader = downloader;
            _errorHandler = errorHandler;
        }

        /// <summary>
        /// Begins a backup of the current SalesForce.com data.
        /// </summary>
        public void Run()
        {
            try
            {
                var files = _downloader.Download();
                _filesToDelete.AddRange(files);

                foreach (var file in files.Select(RenameFile))
                {
                    _filesToDelete.Add(file);
                    _uploader.Upload(file);
                }
            }
            catch (Exception e)
            {
                _errorHandler.HandleError(e);
            }
            finally
            {
                try
                {
                    foreach (var file in _filesToDelete.Where(File.Exists))
                    {
                        File.Delete(file);
                    }
                }
                catch (Exception e)
                {
                    _errorHandler.HandleError(e);
                }
            }
        }

        /// <summary>
        /// Renames a file to match the required backup storage pattern.
        /// </summary>
        /// <param name="file">The file to rename.</param>
        /// <returns>The full name and filepath of the new file.</returns>
        /// <remarks>Uses the following pattern: salesforce/YYYY-MM-DD_HH-MM.* </remarks>
        private string RenameFile(string file)
        {
            var i = -1;
            string newFile;
            
            do
            {
                newFile = String.Join(Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture),
                    new[] { Path.GetDirectoryName(file), FormatFileName(Path.GetExtension(file), ++i) });
            } while (File.Exists(newFile));

            File.Move(file, newFile);

            return newFile;
        }

        /// <summary>
        /// Formats the name of the file.
        /// </summary>
        /// <param name="extension">The extension to apply to the filename.</param>
        /// <param name="i">The revision number to apply to the filename.</param>
        /// <returns>The formatted filename.</returns>
        /// <remarks>If the revision is less than or equal to 0, then no revision indicator will be applied.</remarks>
        private string FormatFileName(string extension, int i)
        {
            var revision = i <= 0 ? String.Empty : String.Format("-{0}", i);
            var dateName = String.Format("{0}{1}{2}", GetDatetimeString(), revision, extension);
            return dateName;
        }

        /// <summary>
        /// Gets a specially formatted datetime string based on the current UTC datetime.
        /// </summary>
        /// <returns>The datetime string.</returns>
        private string GetDatetimeString()
        {
            return String.Format("{0}-{1}-{2}_{3}-{4}",
                DateTime.UtcNow.Year.ToString("D4"),
                DateTime.UtcNow.Month.ToString("D2"),
                DateTime.UtcNow.Day.ToString("D2"),
                DateTime.UtcNow.Hour.ToString("D2"),
                DateTime.UtcNow.Minute.ToString("D2"));
        }

    }
}
