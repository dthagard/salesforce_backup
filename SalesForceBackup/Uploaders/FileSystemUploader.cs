using SalesForceBackup.Interfaces;
using System;
using System.IO;
using TinyIoC;

namespace SalesForceBackup.Uploaders
{

#pragma warning disable CA1305

    public class FileSystemUploader : IUploader
    {
        public void Upload(string file, string targetName) {
            IAppSettings appSettings = TinyIoCContainer.Current.Resolve<IAppSettings>();
            string outputFolder = appSettings.Get(AppSettingKeys.OutputDirectory);
            string destination = String.Join(Path.DirectorySeparatorChar.ToString(), new[] { outputFolder, targetName });
            File.Move(file, destination);
        }
    }

#pragma warning restore CA1305
}
